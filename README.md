# MovieOnline App

Provides Api to extract and modify movies catalog

## Technologies

The technologies design consists of
   * Database - Microsoft SQL Server to store movies catalog
   * Orleans - Develop using Orleans 3.X 
   * Rest Api - Develop using .Net 8
   * GraphQL - Develop using GraphQL 3.X
   * Api documentation - Develop using swagger

## Application Logic

The Movie Online App uses Orleans grain virtual actor model to model grains for different purpose

###  1. Movie Grain

Summary : Represents each movie in the catalog

![MovieGrain](https://github.com/chuaxiangjie/MovieOnline/assets/5947398/aecb5d29-6ef9-4229-ab9f-4308771e273e)


| Features | | Details |
| :---:       |     :---:      |          :---: |
| Grain Key   | ✓   |  Movie Key (string) <br></br>Example : `pacific-rim`  |
| In-Memory persistance state   | ✓   |  Store movie state   |
| External persistance | ✓  | Read/Store movie in relational database   |
| Grain Activation | ✓   | Activate only on demand. <br><br>If memory state is null, fetch movie from external datasource and update state   |
| Get movie    | ✓      | Read from memory state   |
| Create movie    | ✓     | Write to memory state and external persistance   |
| Update movie    | ✓     | Optimistic concurrency check via etag. <br>Update memory state and external persistance    |
| Publish event  |  ✓   |  On every successful create/update, publish *MovieCreatedOrUpdatedEvent*   |

###  2. Movie Search Indexer Grain

Summary : Represents each unique search request based on keys


![MovieSearchIndexerGrain](https://github.com/chuaxiangjie/MovieOnline/assets/5947398/6d222bbe-6f1d-4442-bae7-3fddeef6ae5f)


| Features |  | Details |
| :---:       |     :---:      |          :---: |
| Grain Key   | ✓  |  {name}_{genre} (string) <br><br>Example: `avenger_action`, `aven_`, `adventure` <br><br>Client search for name, genre which are then formatted as key, the above will results in activation of 3 grains  |
| In-Memory Cache   | ✓    |  Stores the search results queries from external datasource.   |
| External datasource | ✓   | Query movies from relational database   |
| Grain Activation | ✓   | Activate only on demand. <br><br>Subscribe to *MovieCreatedOrUpdatedEvent*   |
| GetMany    | ✓      | Fetch from memory cache if exist, else, query from external datasource and store in cache <br><br> Queried using `name`, `genre`, `pagesize`, `referenceId`   |
| Consuming event  |  ✓   | Listens to *MovieCreatedOrUpdatedEvent* and clear all memory cache    |


###  3. Movie Top Rating Indexer Grain

Summary : Represents each unique search request based on keys

| Features |  | Details |
| :---:       |     :---:      |          :---: |
| Grain Key   | ✓  |  {top_number_of_records} (int) <br><br>Example: `5`, `10`, `20`, `30`  |
| In-Memory Cache   | ✓    |  Stores the search results queries from external datasource.   |
| External datasource | ✓   | Query movies from relational database   |
| Grain Activation | ✓   | Activate only on demand. <br><br>Subscribe to *MovieCreatedOrUpdatedEvent*   |
| GetMany    | ✓      | Fetch from memory cache if exist, else, query from external datasource and store in cache <br><br> Queried using `top_number_of_records`   |
| Consuming event  |  ✓   | Listens to *MovieCreatedOrUpdatedEvent* and clear all memory cache    |


## Getting Started

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes.

### Prerequisites

What things you need to install the software and how to install them

#### Windows Environment (With Visual Studio)

1. Install Visual Studio 2022 Community 
(https://visualstudio.microsoft.com/vs/community/)
> [!NOTE]  
> Must be above v17.8+ to support .Net 8

2. Install Microsoft SQL Server Management Studio (SSMS) <br>
Follow tutorial : https://www.c-sharpcorner.com/article/how-to-install-sql-server-20222/

3. Clone repository in visual studio
   
4. Update database connection string in application
> [!NOTE]
> Only server name is required to change

![image](https://github.com/chuaxiangjie/MovieOnline/assets/5947398/1d40de17-1f0f-47cd-b34c-7313585beca5)

5. Open command prompt, enter
```
dotnet tool install --global dotnet-ef
```

6. In the same command prompt, cd to Movies.Database folder, enter
```
dotnet ef database update
```

7. Verify in Microsoft SQL Server Management Studio (SSMS), database MoviesFromJson is created with Movies table seeded with sample data.

8. Build and Run using Visual Studio 2022
```
-> Clone repository using VS, build and run application
```

#### Execute Apis via swagger

Browse swagger url : http://localhost:6600/swagger/index.html

![image](https://github.com/chuaxiangjie/MovieOnline/assets/5947398/c7ac5427-36bf-4763-a376-dd624e38d38b)

