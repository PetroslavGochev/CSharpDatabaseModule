namespace BookShop
{
    using Data;
    using Initializer;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    public class StartUp
    {
        public static void Main()
        {
            //using var db = new BookShopContext();
            //DbInitializer.ResetDatabase(db);
            var context = new BookShopContext();
            using (context)
            {
                Console.WriteLine(GetAuthorNamesEndingIn(context, Console.ReadLine()));

            }
        }
        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            StringBuilder sb = new StringBuilder();
            using (context)
            {
                var books = context.Books
                    .AsEnumerable()
                    .Where(b => b.AgeRestriction.ToString().ToUpper() == command.ToUpper())
                    .Select(b => new
                    {
                        b.Title
                    })
                    .OrderBy(b => b.Title)
                    .ToList();
                foreach (var item in books)
                {
                    sb.AppendLine(item.Title);
                }
            }
            return sb.ToString().TrimEnd();
        }

        public static string GetGoldenBooks(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();

            using (context)
            {
                var books = context.Books
                    .AsEnumerable()
                    .Where(b => b.Copies < 5000 && b.EditionType.ToString() == "Gold")
                    .Select(b => new
                    {
                        b.Title,
                        b.BookId
                    })
                    .OrderBy(b => b.BookId)
                    .ToList();
                foreach (var book in books)
                {
                    sb.AppendLine(book.Title);
                }
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetBooksByPrice(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();
            using (context)
            {
                var books = context.Books
                    .Where(b => b.Price > 40M)
                    .Select(b => new
                    {
                        b.Title,
                        b.Price
                    })
                    .OrderByDescending(b => b.Price)
                    .ToList();
                foreach (var book in books)
                {
                    sb.AppendLine($"{book.Title} - ${book.Price}");
                }
            }
            return sb.ToString().TrimEnd();
        }

        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            StringBuilder sb = new StringBuilder();

            using (context)
            {
                var books = context.Books
                    .Where(b => b.ReleaseDate.Value.Year != year)
                    .OrderBy(b => b.BookId)
                    .Select(b => new
                    {
                        b.Title
                    })
                    .ToList();

                foreach (var book in books)
                {
                    sb.AppendLine(book.Title);
                }
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            StringBuilder sb = new StringBuilder();
            List<string> result = new List<string>();
            string[] catergoryArgs = input
                    .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                    .ToArray();
            using (context)
            {
                foreach (var category in catergoryArgs)
                {
                    var books = context.BooksCategories
                       .Select(bc => new
                       {
                           Title = bc.Book.Title,
                           Name = bc.Category.Name

                       })
                       .Where(b => b.Name == category)
                       .ToList();
                    foreach (var book in books)
                    {
                        result.Add(book.Title);
                    }
                }
            }
            foreach (var book in result.OrderBy(x => x))
            {
                sb.AppendLine(book);
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            StringBuilder sb = new StringBuilder();
            using (context)
            {
                var books = context.Books
                    .Where(b => b.ReleaseDate.Value < DateTime.Parse(date))
                    .OrderByDescending(b => b.ReleaseDate)
                    .Select(b => new
                    {
                        Title = b.Title,
                        EditionType = b.EditionType,
                        Price = b.Price
                    })
                    .ToList();

                foreach (var book in books)
                {
                    sb.AppendLine($"{book.Title} - {book.EditionType} - ${book.Price}");
                }
            }
            return sb.ToString().TrimEnd();
        }

        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            StringBuilder sb = new StringBuilder();
            using (context)
            {
                var authors = context.Authors
                    .Where(a => a.FirstName.EndsWith(input))
                    .Select(a => new
                    {
                        FullName = a.FirstName + " " + a.LastName
                    })
                    .OrderBy(a => a.FullName)
                    .ToList();
                foreach (var a in authors)
                {
                    sb.AppendLine(a.FullName);
                }
            }
            return sb.ToString().TrimEnd();
        }
    }
}

