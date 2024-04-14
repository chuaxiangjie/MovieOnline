using Movies.Contracts.Events;
using Orleans;
using Orleans.Streams;
using System;
using System.Threading.Tasks;

namespace Movies.Grains
{
	public abstract class MovieIndexerGrainBase : Grain
	{
		protected MovieIndexerGrainBase() 
		{
		}

		protected async Task SubscribeToMovieCreatedOrUpdatedEventAsync(Action onReceived)
		{
			// Create a GUID based on our GUID as a grain
			var guid = new Guid("0240de60-ddde-4b75-b183-0633966ab72e");

			// Get one of the providers which we defined in config
			var streamProvider = GetStreamProvider("SMSProvider");

			// Get the reference to a stream
			var stream = streamProvider.GetStream<MovieCreatedOrUpdatedEvent>(guid, "movieapp");

			// Set our OnNext method to the lambda which simply prints the data.
			await stream.SubscribeAsync<MovieCreatedOrUpdatedEvent>(
				async (data, token) =>
				{
					onReceived();

					await Task.CompletedTask;
				});
		}
	}
}