using Demo_IoC.Model;
using System.Text.Json;

namespace Demo_IoC
{
    public class JSONMovieReader : IMovieReader
    {
        static string file = @"Data\MoviesDB.json";
        static List<Movie> movies = new List<Movie>();

        public List<Movie> ReadMovies()
        {
            var moviesText = File.ReadAllText(file);
            return JsonSerializer.Deserialize<List<Movie>>(moviesText);
        }
    }
}
