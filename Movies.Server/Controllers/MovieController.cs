using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Movies.Contracts.Dtos;
using Movies.Contracts.MovieGrains;
using Movies.Contracts.MovieIndexerGrains;
using Movies.Server.Mappers;
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
		public async Task<IActionResult> GetAllAsync([FromQuery] GetMoviesInput getMoviesInput)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var movies = await movieIndexingGrainClient.GetAllAsync(getMoviesInput);
			var output = movies.Select(x => x.ToMovieOutput()).ToList();

			return Ok(output);
		}

		// GET api/movies/top-rated
		[HttpGet("top-rated")]
		public async Task<IActionResult> GetTopRatedAsync([FromQuery] GetTopRatedMoviesInput getTopRatedMoviesInput)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var movies = await movieIndexingGrainClient.GetTopRatedAsync(getTopRatedMoviesInput);
			var output = movies.Select(x => x.ToMovieOutput()).ToList();

			return Ok(output);
		}

		// POST api/movies
		[Authorize]
		[HttpPost]
		public async Task<IActionResult> CreateAsync([FromBody] CreateMovieInput createMovieInput)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

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