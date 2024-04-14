using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Movies.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Movies.Server.Controllers
{
	[Route("api/[controller]")]
	public class MovieController(
		IMovieGrainClient client,
		IMovieIndexingGrainClient movieIndexingGrainClient
		) : Controller
	{
		// GET api/movie/avenger
		[HttpGet("{key}")]
		public async Task<IActionResult> GetAsync([FromRoute] string key)
		{
			var movie = await client.GetAsync(key);

			if (movie is null)
			{
				return NotFound();
			}

			return Ok(movie);
		}

		[HttpGet]
		public async Task<List<Movie>> GetAllAsync([FromQuery] GetMoviesInput getMoviesInput)
		{
			var movies = await movieIndexingGrainClient.GetAllAsync(getMoviesInput);

			return movies;
		}

		[Authorize]
		[HttpPost]
		public async Task<IActionResult> CreateAsync([FromBody] CreateMovieInput createMovieInput)
		{
			var (isSuccess, reason) = await client.CreateAsync(createMovieInput);

			if (!isSuccess)
			{
				return BadRequest(reason);
			}

			return Ok();
		}

		[Authorize]
		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateAsync([FromRoute] string movieKey, [FromBody] CreateMovieInput createMovieInput)
		{
			return Ok();
		}
	}
}