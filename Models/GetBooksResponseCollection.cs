using System.Collections.Generic;

namespace LibraryApi.Models
{
    public class GetBooksResponseCollection
    {
        public List<BookSummaryItem> Books { get; set; }
        public string GenreFilter { get; set; }
    }

    public class BookSummaryItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Genre { get; set; }
    }
}