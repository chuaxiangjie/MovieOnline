using Movies.Contracts.Events;
using Orleans;
using Orleans.Streams;
using System;
using System.Threading.Tasks;

namespace Movies.Grains
{
	public abstract class MovieAppGrainBase<TState> : Grain<TState>
	{
		protected MovieAppGrainBase() 
		{
		}

		protected async Task SubscribeToMovieCreatedOrUpdatedEventAsync(Action onReceived)
		{
			var stream = GetMovieCreatedOrUpdatedEventStream();
			await stream.SubscribeAsync<MovieCreatedOrUpdatedEvent>(
				async (data, token) =>
				{
					onReceived();

					await Task.CompletedTask;
				});
		}

		protected Task PublishToMovieCreatedOrUpdatedEventAsync(MovieCreatedOrUpdatedEvent eventToPublish)
		{
			var stream = GetMovieCreatedOrUpdatedEventStream();

			stream.OnNextAsync(eventToPublish);

			return Task.CompletedTask;
		}

		private IAsyncStream<MovieCreatedOrUpdatedEvent> GetMovieCreatedOrUpdatedEventStream()
		{
			// Create a GUID based on our GUID as a grain
			var guid = new Guid("0240de60-ddde-4b75-b183-0633966ab72e");

			// Get the stream provider - name is fixed
			var streamProvider = GetStreamProvider("SMSProvider");

			// Get the reference to a stream
			var stream = streamProvider.GetStream<MovieCreatedOrUpdatedEvent>(guid, "movieapp");

			return stream;
		}
	}
}