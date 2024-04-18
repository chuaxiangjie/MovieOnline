using Orleans;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Movies.Domain;
using Movies.Database;
using Movies.Contracts.MovieIndexerGrains;
using System.Linq;

namespace Movies.Grains
{
	public class MovieSearchIndexerGrain : MovieIndexerGrainBase, IMovieSearchIndexerGrain
	{
		private readonly IMovieRepository _movieRepository;
		private bool _requiresMoviesRefresh = false;

		// Key refers to "{pageSize},{referenceId}"
		// Value refers to "(nextReferenceId, List of movieIds)"
		private readonly Dictionary<string, (int nextReferenceId, List<int> movieIds)> _moviesPageSizeReferenceIdCache;
		private readonly Dictionary<int, MovieBasicInfo> _moviesBasicInfoCache;

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

				PopulateToCache(cacheKey, movieBasicInfoPagedResponse);

				return movieBasicInfoPagedResponse;
			}
		}

		private void PopulateToCache(string cacheKey, PagedResponseKeyset<MovieBasicInfo> moviesBasicInfoPagedResponse)
		{
			foreach (var movie in moviesBasicInfoPagedResponse.Data)
			{
				_moviesBasicInfoCache.TryAdd(movie.Id, movie);
			}

			var nextReferenceId = moviesBasicInfoPagedResponse.ReferenceId;
			_moviesPageSizeReferenceIdCache.Add(cacheKey, (nextReferenceId, _moviesBasicInfoCache.Keys.ToList()));
		}

		private (string name, Genre? genre) GetSearchParameters()
		{
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