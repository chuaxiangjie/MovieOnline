using Microsoft.EntityFrameworkCore;
using Movies.Contracts;

namespace Movies.Server.EntityFrameworkCore
{
	public class MovieDbContext : DbContext
	{
		public MovieDbContext(DbContextOptions<MovieDbContext> options) : base(options) { }

		public DbSet<Movie> Movies { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			if (!optionsBuilder.IsConfigured)
			{
			}
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Movie>(entity =>
			{
				
			});
		}
	}
}
