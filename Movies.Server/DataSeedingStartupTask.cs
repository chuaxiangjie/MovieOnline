using Orleans.Runtime;
using Orleans;
using System.Threading.Tasks;
using System.Threading;
using Movies.Contracts.MovieGrains;

namespace Movies.Server
{
	public sealed class DataSeedingStartupTask(IGrainFactory grainFactory) : IStartupTask
	{
		public async Task Execute(CancellationToken cancellationToken)
		{
			
		}
	}
}