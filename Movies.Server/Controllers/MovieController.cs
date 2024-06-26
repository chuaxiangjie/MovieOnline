﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Movies.Contracts.Dtos;
using Movies.Contracts.MovieGrains;
using Movies.Contracts.MovieIndexerGrains;
using Movies.Server.Helpers;
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

		/// <summary>
		/// Get movie details
		/// </summary>
		/// <param name="key"></param>
		/// <response code="200"></response>
		/// <response code="400"></response>
		[HttpGet("{key}")]
		[ProducesResponseType(typeof(MovieOutput),StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
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

		/// <summary>
		/// Search movies
		/// </summary>
		/// <param name="getMoviesInput"></param>
		/// <response code="200"></response>
		/// <response code="400"></response>
		[HttpGet]
		[ProducesResponseType(typeof(MoviePagedOutput<MovieBasicInfoOutput>), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> GetAllAsync([FromQuery] GetSearchMoviesInput getMoviesBasicInfoInput)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var pagedResponse = await movieIndexingGrainClient.GetAllAsync(getMoviesBasicInfoInput);
			var output = pagedResponse.ToMoviePagedOutput();

			return Ok(output);
		}

		/// <summary>
		/// Get top-rated movies
		/// </summary>
		/// <param name="getTopRatedMoviesInput"></param>
		/// <response code="200"></response>
		/// <response code="400"></response>
		[HttpGet("top-rated")]
		[ProducesResponseType(typeof(List<MovieBasicInfoOutput>), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> GetTopRatedAsync([FromQuery] GetTopRatedMoviesInput getTopRatedMoviesInput)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var movies = await movieIndexingGrainClient.GetTopRatedAsync(getTopRatedMoviesInput);
			var output = movies.Select(x => x.ToMovieBasicInfoOutput()).ToList();

			return Ok(output);
		}

		/// <summary>
		/// Create movie
		/// </summary>
		/// <param name="createMovieInput"></param>
		/// <response code="200"></response>
		/// <response code="400"></response>
		[Authorize]
		[HttpPost]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
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

		/// <summary>
		/// Update movie
		/// </summary>
		/// <param name="key"></param>
		/// <param name="updateMovieInput"></param>
		/// <response code="200"></response>
		/// <response code="400"></response>
		[Authorize]
		[HttpPut("{key}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> UpdateAsync([FromRoute] string key, [FromBody] UpdateMovieInput updateMovieInput)
		{
			var etag = HttpContext.Request.GetETag();
			var (isSuccess, reason) = await client.UpdateAsync(key, etag, updateMovieInput);

			if (!isSuccess)
			{
				return BadRequest(reason);
			}

			return Ok();
		}
	}
}