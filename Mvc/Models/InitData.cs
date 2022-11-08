using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Mvc.Data;
using System;
using System.Linq;

namespace Mvc.Models
{
    public static class InitData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using var context = new MvcBookContext(serviceProvider.GetRequiredService<DbContextOptions<MvcBookContext>>());

            InitBook(context);
            InitCity(context);

            context.SaveChanges();
        }

        private static void InitBook(MvcBookContext context)
        {
            if (context.Book.Any()) return;

            context.Book.AddRange(
                new Book
                {
                    Name = "Да здравствует фикус!",
                    Author = "Джордж Оруэлл",
                    Price = 140M,
                },
                new Book
                {
                    Name = "Парень из преисподней",
                    Author = "Братья Стругацкие",
                    Price = 290M,
                },
                new Book
                {
                    Name = "Дни в Бирме",
                    Author = "Джордж Оруэлл",
                    Price = 139M,
                },
                new Book
                {
                    Name = "Философия духа",
                    Author = "Гегель",
                    Price = 407M,
                },
                new Book
                {
                    Name = "Ответ Иову",
                    Author = "Зигмунд Фрейд",
                    Price = 523M,
                },
                new Book
                {
                    Name = "Тотем и табу",
                    Author = "Зигмунд Фрейд",
                    Price = 201M,
                },
                new Book
                {
                    Name = "Путь к основанию",
                    Author = "Зигмунд Фрейд",
                    Price = 752M,
                },
                new Book
                {
                    Name = "Многопоточный JS",
                    Author = "Хантер, Инглиш",
                    Price = 1299M,
                },
                new Book
                {
                    Name = "Человек, который смеется",
                    Author = "Виктор Гюго",
                    Price = 330M,
                },
                new Book
                {
                    Name = "Бремя страстей человеческих",
                    Author = "Сомерсет Моэм",
                    Price = 289M,
                },
                new Book
                {
                    Name = "Преступление и наказание",
                    Author = "Федор Достоевский",
                    Price = 150M,
                },
                new Book
                {
                    Name = "Идиот",
                    Author = "Федор Достоевский",
                    Price = 210M,
                },
                new Book
                {
                    Name = "О дивный новый мир",
                    Author = "Олдос Хаксли",
                    Price = 120M,
                },
                new Book
                {
                    Name = "К востоку от Эдема",
                    Author = "Джон Стейнбек",
                    Price = 340M,
                }
            );
        }

        private static void InitCity(MvcBookContext context)
        {
            if (context.City.Any()) return;

            context.City.AddRange(
                new City { Name = "Москва" },
                new City { Name = "Санкт-Петербург" },
                new City { Name = "Новосибирск" },
                new City { Name = "Екатеринбург" },
                new City { Name = "Казань" },
                new City { Name = "Нижний Новгород" },
                new City { Name = "Челябинск" },
                new City { Name = "Самара" },
                new City { Name = "Омск" },
                new City { Name = "Ростов-на-Дону" },
                new City { Name = "Уфа" },
                new City { Name = "Красноярск" },
                new City { Name = "Воронеж" },
                new City { Name = "Пермь" },
                new City { Name = "Волгоград" }
            );
        }
    }
}
