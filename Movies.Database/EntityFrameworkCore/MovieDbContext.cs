using Microsoft.EntityFrameworkCore;
using Movies.Database.Converter;
using Movies.Database.SeedModel;
using Movies.Domain;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace Movies.Database.EntityFrameworkCore
{
	public class MovieDbContext : DbContext
	{
		public MovieDbContext()
		{
		}

		public MovieDbContext(DbContextOptions<MovieDbContext> options) : base(options) { }

		public DbSet<Movie> Movies { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer("Server=LAPTOP-PJ8829TH\\SQLEXPRESS;Database=MovieStore;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=Yes;");
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			var converter = new GenreListConverter();

			modelBuilder.Entity<Movie>(entity =>
			{
				entity.Property(b => b.Key)
				.HasMaxLength(100)
				.IsRequired();

				entity.Property(b => b.Name)
				.HasMaxLength(100)
				.IsRequired();

				entity.Property(b => b.Description)
				.HasMaxLength(500)
				.IsRequired();

				entity
				.Property(b => b.Genres)
				.HasConversion(converter)
				.HasMaxLength(100)
				.IsRequired();

				entity.Property(b => b.Rate)
				.HasPrecision(2, 1)
				.IsRequired();

				entity.Property(b => b.Length)
				.HasMaxLength(50)
				.IsRequired();

				entity.Property(b => b.Img)
				.HasMaxLength(255)
				.IsRequired();

				entity.HasData(SeedMovieData());
			});
		}

		private static List<Movie> SeedMovieData()
		{
			var movieStore = new MovieStore();
			using (var r = new StreamReader(@"../movies.json"))
			{
				var json = r.ReadToEnd();
				movieStore = JsonConvert.DeserializeObject<MovieStore>(json);
			}
			return movieStore.Movies;
		}
	}
}