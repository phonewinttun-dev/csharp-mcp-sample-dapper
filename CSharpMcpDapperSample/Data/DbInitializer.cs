using CSharpMcpDapperSample.Repository;

namespace CSharpMcpDapperSample.Data
{
    public class DbInitializer
    {
        public static async Task Initialize(BookRepository bookService)
        {
            var books = new List<(string Title, string Author, string Genre)>
            {
                ("The City of Joy", "Dominique Lapierre", "Fiction"),
                ("The Fall", "Albert Camus", "Fiction"),
                ("1984", "George Orwell", "Dystopian"),
                ("To Kill a Mockingbird", "Harper Lee", "Fiction"),
                ("Brave New World", "Aldous Huxley", "Dystopian"),
                ("The Great Gatsby", "F. Scott Fitzgerald", "Classic"),
                ("Moby Dick", "Herman Melville", "Adventure"),
                ("War and Peace", "Leo Tolstoy", "Historical"),
                ("Crime and Punishment", "Fyodor Dostoevsky", "Philosophical"),
                ("The Alchemist", "Paulo Coelho", "Fiction"),
                ("Sapiens", "Yuval Noah Harari", "Non-Fiction"),
                ("The Hobbit", "J.R.R. Tolkien", "Fantasy")
            };

            foreach (var book in books)
            {
                await bookService.AddAsync(book.Title, book.Author, book.Genre);
            }
        }
    }
}
