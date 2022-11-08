using System.Collections.Generic;

namespace Mvc.Models
{
    public class BookListViewModel
    {
        public List<Book> Books;
        public string SortBy;
        public string SortDirection;
        public string Search;
    }
}
