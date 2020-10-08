//2. Как можно оптимизировать следующий код:
//. Как можно уменьшить сложность с O(n^2) до O(n)? 
var movies = Enumerable.Range(0, 10000)
	.Select(i => new Movie {Id = i, Title = $"Movie{i}"})
	.ToList();
var sessions = Enumerable.Range(1, 100000)
	.Select(i => new Session {Id = i, MovieId = i / 10})
	.ToList();
foreach (var session in sessions)
{
	session.MovieTitle = movies
		.FirstOrDefault(movie => movie.Id == session.MovieId)?.Title;
}

	class Session
    	{
        	public int Id { get; set; }
        	public int MovieId { get; set; }
        	public string MovieTitle { get; set; }
    	}
	class Movie
    	{
        	public int Id { get; set; }
        	public string Title { get; set; }
    	}



//Решение:

Movie[] movies = Enumerable.Range(0, 10000)
	.Select(i => new Movie { Id = i, Title = $"Movie{i}" }).ToArray();
Session[] sessions = Enumerable.Range(1, 100000)
        .Select(i => new Session { Id = i, MovieId = i / 10}).ToArray();
foreach (var session in sessions)
{
	try
        {
        	session.MovieTitle = movies[session.MovieId].Title;
        }
        catch (IndexOutOfRangeException)
        {
        	session.MovieTitle = null;
        }
}
