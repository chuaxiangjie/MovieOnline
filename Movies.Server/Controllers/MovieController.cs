using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Movies.Contracts.Dtos;
using Movies.Contracts.MovieGrains;
using Movies.Contracts.MovieIndexerGrains;
using Movies.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movies.Server.Controllers
{
	[Route("api/movies")]
	public class MovieController(
		IMovieGrainClient client,
		IMovieIndexerGrainClient movieIndexingGrainClient
		) : Controller
	{
		const string ETAG_HEADER = "ETag";
		const string MATCH_HEADER = "If-Match";

		// GET api/movie/avenger
		[HttpGet("{key}")]
		public async Task<IActionResult> GetAsync([FromRoute] string key)
		{
			var (movie, etag) = await client.GetAsync(key);

			if (movie is null)
			{
				return NotFound();
			}

			HttpContext.Response.Headers.Add(ETAG_HEADER, etag);

			return Ok(movie);
		}

		[HttpGet]
		public async Task<List<Movie>> GetAllAsync([FromQuery] GetMoviesInput getMoviesInput)
		{
			var movies = await movieIndexingGrainClient.GetAllAsync(getMoviesInput);

			return movies;
		}

		[HttpGet("top-rated")]
		public async Task<List<Movie>> GetTopRatedAsync([FromQuery] GetTopRatedMoviesInput getTopRatedMoviesInput)
		{
			var movies = await movieIndexingGrainClient.GetTopRatedAsync(getTopRatedMoviesInput);

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
		[HttpPut("{key}")]
		public async Task<IActionResult> UpdateAsync([FromRoute] string key, [FromBody] UpdateMovieInput updateMovieInput)
		{
			HttpContext.Request.Headers.TryGetValue(MATCH_HEADER, out var value);
			var etag = value.FirstOrDefault();
			
			var (isSuccess, reason) = await client.UpdateAsync(key, etag, updateMovieInput);

			if (!isSuccess)
			{
				return BadRequest(reason);
			}

			return Ok();
		}
	}
}