using Microsoft.EntityFrameworkCore;
using Movies.Database.EntityFrameworkCore;
using Movies.Database.Extensions;
using Movies.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movies.Database
{
	public class MovieRepository(IDbContextFactory<MovieDbContext> contextFactory) : IMovieRepository
	{
		public async Task<PagedResponseKeyset<MovieBasicInfo>> GetAllAsync(
			int pageSize, int referenceId, string name, Genre? genre)
		{
			using (var context = contextFactory.CreateDbContext())
			{
				var queryable = context.Movies
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
		}

		public async Task<IEnumerable<MovieBasicInfo>> GetTopRatedAsync(int numberOfTopRatedMovie)
		{
			using (var context = contextFactory.CreateDbContext())
			{
				var queryable = context.Movies
				.AsQueryable().AsNoTracking()
				.OrderByDescending(x => x.Rate)
				.Take(numberOfTopRatedMovie)
				.Select(x => new MovieBasicInfo { Id = x.Id, Key = x.Key, Name = x.Name, Genres = x.Genres, Rate = x.Rate });

				var movies = await queryable.ToListAsync();

				return movies;
			}
		}

		public async Task<Movie> GetByKeyAsync(string key)
		{
			using (var context = contextFactory.CreateDbContext())
			{
				return await context.Movies.AsNoTracking()
					.FirstOrDefaultAsync(x => x.Key == key);
			}
		}

		public async Task CreateAsync(Movie movie)
		{
			using (var context = contextFactory.CreateDbContext())
			{
				await context.Movies.AddAsync(movie);
				await context.SaveChangesAsync();
			}
		}

		public async Task UpdateAsync(Movie movie)
		{
			using (var context = contextFactory.CreateDbContext())
			{
				var existingMovie = await context.Movies.FirstAsync(x => x.Id == movie.Id);

				existingMovie.Name = movie.Name;
				existingMovie.Description = movie.Description;

				await context.SaveChangesAsync();
			}
		}
	}
}