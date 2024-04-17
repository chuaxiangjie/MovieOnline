using Orleans;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Movies.Domain;
using Movies.Database;
using Movies.Contracts.MovieIndexerGrains;
using Orleans.Concurrency;
using System.Linq;

namespace Movies.Grains
{
	public class MovieSearchIndexerGrain : MovieIndexerGrainBase, IMovieSearchIndexerGrain
	{
		private readonly IMovieRepository _movieRepository;
		private bool _requiresMoviesRefresh = false;

		// pageSize(5)+refId(2033), [1,4,5,,63232]
		private Dictionary<string, (int nextReferenceId, List<int> movieIds)> _moviesPageSizeReferenceIdCache;
		private Dictionary<int, MovieBasicInfo> _moviesBasicInfoCache;

		public MovieSearchIndexerGrain(IMovieRepository movieRepository)
		{
			_movieRepository = movieRepository;
			_moviesPageSizeReferenceIdCache = [];
			_moviesBasicInfoCache = [];
		}

		public override async Task OnActivateAsync()
		{
			_requiresMoviesRefresh = true;

			await SubscribeToMovieCreatedOrUpdatedEventAsync(() =>
			{
				ClearAllCache();
				_requiresMoviesRefresh = true;
			});
			
			await Task.CompletedTask;
		}

		public async Task<PagedResponseKeyset<MovieBasicInfo>> GetManyAsync(int pageSize, int referenceId)
		{
			var movieBasicInfoPagedResponse = await GetFromCacheOrExternalDatabaseAsync(pageSize, referenceId);

			return movieBasicInfoPagedResponse;
		}

		private async Task<PagedResponseKeyset<MovieBasicInfo>> GetFromCacheOrExternalDatabaseAsync(int pageSize, int referenceId)
		{
			var cacheKey = $"{pageSize},{referenceId}";

			// check if cache key exist
			if (_moviesPageSizeReferenceIdCache.TryGetValue(cacheKey, out var value))
			{
				var (nextReferenceId, movieIds) = value;
				var moviesBasicInfo = _moviesBasicInfoCache.Where(x => movieIds.Contains(x.Key)).Select(x => x.Value).ToList();

				return new PagedResponseKeyset<MovieBasicInfo>(moviesBasicInfo, nextReferenceId);
			}
			else
			{
				var (name, genre) = GetSearchParameters();
				var movieBasicInfoPagedResponse = await _movieRepository.GetAllAsync(pageSize, referenceId, name, genre);

				PopulateToCache(pageSize, referenceId, movieBasicInfoPagedResponse);

				return movieBasicInfoPagedResponse;
			}
		}

		private void PopulateToCache(int pageSize, int referenceId, PagedResponseKeyset<MovieBasicInfo> moviesBasicInfoPagedResponse)
		{
			var cacheKey = $"{pageSize},{referenceId}";

			foreach (var movie in moviesBasicInfoPagedResponse.Data)
			{
				_moviesBasicInfoCache.TryAdd(movie.Id, movie);
			}

			var nextReferenceId = moviesBasicInfoPagedResponse.ReferenceId;

			_moviesPageSizeReferenceIdCache.Add(cacheKey, (nextReferenceId, _moviesBasicInfoCache.Keys.ToList()));
		}

		private (string name, Genre? genre) GetSearchParameters()
		{
			// <Name>,<Genre>
			var searchContentArray = this.GetPrimaryKeyString().Split('_');
			var name = searchContentArray[0];
			Genre? genre = string.IsNullOrEmpty(searchContentArray[1])
				? null
				: (Genre)Enum.Parse(typeof(Genre), searchContentArray[1]);

			return (name, genre);
		}

		private void ClearAllCache()
		{
			_moviesPageSizeReferenceIdCache.Clear();
			_moviesBasicInfoCache.Clear();
		}
	}
}