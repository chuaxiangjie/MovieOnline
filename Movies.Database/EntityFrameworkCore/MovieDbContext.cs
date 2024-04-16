using Microsoft.EntityFrameworkCore;
using Movies.Database.SeedModel;
using Movies.Domain;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
			modelBuilder.Entity<Movie>(entity =>
			{
				entity.HasKey(x => x.Id);
				entity.HasIndex(x => x.Genres);

				entity.Property(b => b.Id)
				.ValueGeneratedOnAdd()
				.UseIdentityColumn();

				entity.Property(b => b.Key)
				.HasMaxLength(100)
				.IsRequired();

				entity.Property(b => b.Name)
				.HasMaxLength(100)
				.IsRequired();

				entity.Property(b => b.Description)
				.HasMaxLength(500)
				.IsRequired();

				entity.Property(b => b.Genres)
				.HasMaxLength(255)
				.IsRequired();

				entity.Property(b => b.Rate)
				.HasPrecision(3, 1)
				.IsRequired();

				entity.Property(b => b.Length)
				.HasMaxLength(50)
				.IsRequired();

				entity.Property(b => b.Img)
				.HasMaxLength(255)
				.IsRequired();

				//entity.HasData(SeedMovieData());
			});
		}

		private static List<Movie> SeedMovieData()
		{
			var movieStore = new MovieStoreFromJson();
			using (var r = new StreamReader(@"../movies.json"))
			{
				var json = r.ReadToEnd();
				movieStore = JsonConvert.DeserializeObject<MovieStoreFromJson>(json);
			}

			var movies = movieStore.Movies.Select(x => ToMovie(x)).ToList();

			return movies;
		}

		private static Movie ToMovie(MovieFromJson movieFromJson)
		{
			return new Movie
			{
				Id = movieFromJson.Id,
				Name = movieFromJson.Name,
				Description = movieFromJson.Description,
				Genres = movieFromJson.Genres,
				Img = movieFromJson.Img,
				Key = movieFromJson.Key,
				Length = movieFromJson.Length,
				Rate = movieFromJson.Rate
			};
		}
	}
}