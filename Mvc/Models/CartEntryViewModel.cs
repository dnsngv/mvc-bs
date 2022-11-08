using System.ComponentModel.DataAnnotations;

namespace Mvc.Models
{
    public class CartEntryViewModel
    {
        public Book Book { get; }
        [Display(Name = "Количество")]
        public int Count { get; }
        [Display(Name = "Сумма")]
        public decimal Total { get; }

        public CartEntryViewModel(Book book, CartEntrySessionJsonModel cartEntry)
        {
            Book = book;
            Count = cartEntry.Count;
            Total = Book.Price * Count;
        }
    }
}
