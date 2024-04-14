﻿using Orleans.Runtime;
using Orleans;
using System.Threading.Tasks;
using System.Threading;
using Movies.Contracts;
using System.Collections.Generic;
using Movies.Database;

namespace Movies.Server
{
	public sealed class DataSeedingStartupTask(IGrainFactory grainFactory, IMovieRepository movieRepository) : IStartupTask
	{
		public async Task Execute(CancellationToken cancellationToken)
		{
			var movies = movieRepository.GetMovies();

			var createMovieTasks = new List<Task>();
			var movieLibraryGrain = grainFactory.GetGrain<IMovieLibraryGrain>(0);

			foreach (var movie in movies)
			{
				movie.Id = 0;

				createMovieTasks.Add(movieLibraryGrain.AddMovieAsync(movie, assignAutoGeneratedId: true));
			}

			await Task.WhenAll(createMovieTasks);
		}
	}
}