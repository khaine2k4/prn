namespace Demo_DI_Constructor_Injection.Model
{
    // Data Access Layer
    public interface IBookReader
    {
        void ReadBooks();
    }

    public class XMLBookReader : IBookReader
    {
        public void ReadBooks()
        {
            Console.WriteLine("Books read in XML Format");
        }
    }

    public class JSONBookReader : IBookReader
    {
        public void ReadBooks()
        {
            Console.WriteLine("Books read in JSON Format");
        }
    }
}
