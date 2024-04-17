using Microsoft.EntityFrameworkCore;
using Movies.Database.EntityFrameworkCore;
using Movies.Database.Extensions;
using Movies.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movies.Database
{
	public class MovieRepository(MovieDbContext movieDbContext) : IMovieRepository, IDisposable
	{
		private readonly MovieDbContext _context = movieDbContext;

		public async Task<PagedResponseKeyset<MovieBasicInfo>> GetAllAsync(
			int pageSize, int referenceId, string name, Genre? genre)
		{
			var queryable = _context.Movies
			.AsQueryable().AsNoTracking()
			.WhereIf(!string.IsNullOrEmpty(name), x => x.Name.Contains(name))
			.WhereIf(genre is not null, x => x.Genres.Any(x => x == genre))
			.Where(x => x.Id > referenceId)
			.Take(pageSize)
			.Select(x => new MovieBasicInfo { Id = x.Id, Key = x.Key, Name = x.Name, Genres = x.Genres, Rate = x.Rate });

			var movies = await queryable.ToListAsync();

			var newReferenceId = movies.Count != 0 ? movies.Last().Id : 0;
			var pagedResponse = new PagedResponseKeyset<MovieBasicInfo>(movies, newReferenceId);

			return pagedResponse;
		}

		public async Task<IEnumerable<MovieBasicInfo>> GetTopRatedAsync(int numberOfTopRatedMovie)
		{
			var queryable = _context.Movies
				.AsQueryable().AsNoTracking()
				.OrderByDescending(x => x.Rate)
				.Take(numberOfTopRatedMovie)
				.Select(x => new MovieBasicInfo { Id = x.Id, Key = x.Key, Name = x.Name, Genres = x.Genres, Rate = x.Rate });

			var movies = await queryable.ToListAsync();

			return movies;
		}

		public async Task<Movie> GetByKeyAsync(string key) => 
			await _context.Movies.AsNoTracking()
				.FirstOrDefaultAsync(x => x.Key == key);

		public async Task CreateAsync(Movie movie)
		{
			await _context.Movies.AddAsync(movie);
			await SaveAsync();
		}

		public async Task UpdateAsync(Movie movie)
		{
			var existingMovie = await _context.Movies.FirstAsync(x => x.Id == movie.Id);

			existingMovie.Name = movie.Name;
			existingMovie.Description = movie.Description;

			await SaveAsync();
		}

		private async Task SaveAsync() => await _context.SaveChangesAsync();

		private bool disposed = false;

		protected virtual void Dispose(bool disposing)
		{
			if (!disposed)
			{
				if (disposing)
				{
					_context.Dispose();
				}
			}
			disposed = true;
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
	}
}