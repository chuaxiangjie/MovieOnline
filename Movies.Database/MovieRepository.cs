using Microsoft.EntityFrameworkCore;
using Movies.Database.EntityFrameworkCore;
using Movies.Database.Extensions;
using Movies.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Movies.Database
{
	public class MovieRepository(MovieDbContext context) : IMovieRepository, IDisposable
	{
		private readonly MovieDbContext _context = context;

		public async Task<IEnumerable<Movie>> GetAllAsync(string name = null, Genre? genre = null)
		{
			var queryable = _context.Movies.AsQueryable()
				.WhereIf(!string.IsNullOrEmpty(name), x => x.Name.Contains(name))
				.WhereIf(genre is not null, x => x.GenresAsString.Contains(genre.ToString()));

			var movies = await queryable.ToListAsync();

			return movies;
		}

		public async Task<Movie> GetByKeyAsync(string key) => await _context.Movies.FirstOrDefaultAsync(x => x.Key == key);

		public async Task CreateAsync(Movie movie)
		{
			await _context.Movies.AddAsync(movie);
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