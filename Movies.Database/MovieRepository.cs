using Movies.Database.EntityFrameworkCore;
using Movies.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Movies.Database
{
	public interface IMovieRepository : IDisposable
	{
		IEnumerable<Movie> GetMovies();
	}

	public class MovieRepository(MovieDbContext context) : IMovieRepository, IDisposable
	{
		private readonly MovieDbContext _context = context;

		public IEnumerable<Movie> GetMovies() => _context.Movies.ToList();

		public void Save() => _context.SaveChanges();

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