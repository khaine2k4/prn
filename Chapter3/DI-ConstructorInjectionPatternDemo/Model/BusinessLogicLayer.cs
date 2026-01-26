using Demo_DI_Constructor_Injection.Model;

namespace Demo_DI__Constructor_Injection.Model
{
    // Business Logic Layer
    public class BookManager
    {
        public IBookReader bookReader;

        // Constructor Injection
        public BookManager(IBookReader reader)
        {
            bookReader = reader;
        }

        public void ReadBooks()
        {
            bookReader.ReadBooks();
        }
    }
}
