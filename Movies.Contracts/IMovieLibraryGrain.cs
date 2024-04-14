﻿using Movies.Domain;
using Orleans;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Movies.Contracts
{
	public interface IMovieLibraryGrain : IGrainWithIntegerKey
	{
		Task<List<Movie>> GetAllAsync(string name = null, Genre? genre = null);
		Task<(bool isSuccess, string reason)> AddMovieAsync(Movie movie, bool assignAutoGeneratedId);
	}
}