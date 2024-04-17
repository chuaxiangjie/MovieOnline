using Orleans.Runtime;
using Orleans;
using System.Threading.Tasks;
using System.Threading;
using Movies.Contracts.MovieGrains;
using Movies.Database;
using Bogus;
using Movies.Domain;
using System.Collections.Generic;
using System;
using Movies.Database.EntityFrameworkCore;
using EFCore.BulkExtensions;
using System.Linq;
using Movies.Contracts;
using Movies.Contracts.MovieIndexerGrains;

namespace Movies.Server
{
	public sealed class GrainInitializationStartupTask(MovieDbContext movieDbContext,
		IGrainFactory grainFactory, IMovieRepository movieRepository) : IStartupTask
	{
		public async Task Execute(CancellationToken cancellationToken)
		{
			var allTopMovieRatingRanges = (TopMovieRatingRange[])Enum.GetValues(typeof(TopMovieRatingRange));

			foreach (var ratingRange in allTopMovieRatingRanges)
			{
				var movieTopRatingIndexerGrain = grainFactory.GetGrain<IMovieTopRatingIndexerGrain>((int)ratingRange);
				await movieTopRatingIndexerGrain.GetManyAsync();
			}
		}

		//public async Task Execute(CancellationToken cancellationToken)
		//{
		//	//await movieRepository.FakeAsync();

		//	//var moviesData = FakeMovieData();

		//	////foreach (var movie in moviesData)
		//	////{
		//	////	movieDbContext.Movies.Add(movie);
		//	////}

		//	//movieDbContext.BulkInsert(moviesData);

		//	//movieDbContext.SaveChanges();
		//}

		private static List<Movie> FakeMovieData()
		{
			var allGenres = (Genre[])Enum.GetValues(typeof(Genre));

			var faker = new Faker<Movie>()
			.RuleFor(m => m.Key, f => f.Random.AlphaNumeric(80))
			.RuleFor(m => m.Name, f => f.Random.Word())
			.RuleFor(m => m.Description, f => f.Lorem.Paragraph())
			.RuleFor(m => m.Genres, f => f.PickRandom(allGenres, 3).ToArray())
			.RuleFor(m => m.Rate, f => f.Random.Decimal(0, 10))
			.RuleFor(m => m.Length, f => f.Random.Number(60, 180) + " mins")
			.RuleFor(m => m.Img, f => f.Image.PicsumUrl());

			var movies = faker.Generate(1000000);

			return movies;
		}
	}
}