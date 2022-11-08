using System.ComponentModel.DataAnnotations;

namespace Mvc.Models
{
    public class Book
    {
        public int Id { get; set; }
        [Display(Name = "Название")]
        public string Name { get; set; }
        [Display(Name = "Автор")]
        public string Author { get; set; }
        [Display(Name = "Цена")]
        public decimal Price { get; set; }
    //  [Display(Name = "Тип литературы")]
    //  public BookType Type { get; set; }
    //  [Display(Name = "Жанр")]
    //  public BookGenre Genre { get; set; }
    }

    //   public enum BookType
    //   {
    //   [Display(Name = "Художественная")]
    //   Art,
    //   [Display(Name = "Научно-популярная")]
    //   Science,
    //}

    //public enum BookGenre
    //{
    //    [Display(Name = "Роман")]
    //    Novel,
    //    [Display(Name = "Трагикомедия")]
    //    Tragicomedy,
    //    [Display(Name = "Гуманитарные науки")]
    //    Human,
    //    [Display(Name = "Технические науки")]
    //    Tech,
    //}
}
