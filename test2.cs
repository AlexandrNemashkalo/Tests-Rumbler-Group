//2. Как можно оптимизировать следующий код:
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


/*Ответ: 
Код можно оптимизировать с помощью добавления в класс Session ссылку на класс Movie,
это позволит нам не копировать и не хранить повторяющиеся данные (MovieId, MovieTitle)
*/
//Решение:

var movies = Enumerable.Range(0, 10000)
	.Select(i => new Movie { Id = i, Title = $"Movie{i}" })
        .ToList(); 

var sessions = Enumerable.Range(1, 100000)
	.Select(i => new Session { Id = i, Movie = movies.FirstOrDefault(x=> x.Id == i/10)})
	.ToList();
 
    	class Session
    	{
        	public int Id { get; set; }
        	public Movie Movie { get; set; }
    	}

	class Movie
    	{
        	public int Id { get; set; }
        	public string Title { get; set; }
    	}
