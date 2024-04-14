using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Movies.Contracts.Dtos;
using Movies.Contracts.MovieGrains;
using Movies.Contracts.MovieIndexerGrains;
using Movies.Domain;
using Movies.Server.Mappers;
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

		// GET api/movies/avenger
		[HttpGet("{key}")]
		public async Task<IActionResult> GetAsync([FromRoute] string key)
		{
			var (movie, etag) = await client.GetAsync(key);

			if (movie is null)
			{
				return NotFound();
			}

			HttpContext.Response.Headers.Add(ETAG_HEADER, etag);

			return Ok(movie.ToMovieOutput(etag));
		}

		// GET api/movies
		[HttpGet]
		public async Task<List<MovieOutput>> GetAllAsync([FromQuery] GetMoviesInput getMoviesInput)
		{
			var movies = await movieIndexingGrainClient.GetAllAsync(getMoviesInput);
			var output = movies.Select(x => x.ToMovieOutput()).ToList();

			return output;
		}

		// GET api/movies/top-rated
		[HttpGet("top-rated")]
		public async Task<List<MovieOutput>> GetTopRatedAsync([FromQuery] GetTopRatedMoviesInput getTopRatedMoviesInput)
		{
			var movies = await movieIndexingGrainClient.GetTopRatedAsync(getTopRatedMoviesInput);
			var output = movies.Select(x => x.ToMovieOutput()).ToList();

			return output;
		}

		// POST api/movies
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

		// PUT api/movies/avenger
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