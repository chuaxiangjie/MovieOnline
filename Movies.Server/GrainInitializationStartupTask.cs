using Orleans.Runtime;
using Orleans;
using System.Threading.Tasks;
using System.Threading;
using System;
using Movies.Contracts;
using Movies.Contracts.MovieIndexerGrains;

namespace Movies.Server
{
	public sealed class GrainInitializationStartupTask(IGrainFactory grainFactory) : IStartupTask
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
	}
}