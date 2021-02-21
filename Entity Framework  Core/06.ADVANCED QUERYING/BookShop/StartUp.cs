namespace BookShop
{
    using Data;
    using Initializer;
    using Microsoft.EntityFrameworkCore;
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
                Console.WriteLine(GetBooksByCategory(context,Console.ReadLine()));
            }
        }

        //2. Age Restriction
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

        //3. Golden Books
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

        //4. Books by Price
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

        //5. Not Released In
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

        //6. Book Titles by Category
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

        //7. Released Before Date
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

        //8. Author Search
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

        //9. Book Search
        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            StringBuilder sb = new StringBuilder();
            using (context)
            {
                var books = context.Books
                    .Where(b => b.Title.Contains(input))
                    .OrderBy(b => b.Title)
                    .Select(b => new
                    {
                        Title = b.Title
                    })
                    .ToList();
                foreach (var book in books)
                {
                    sb.AppendLine(book.Title);
                }
            }
            return sb.ToString().TrimEnd();
        }

        //10. Book Search by Author
        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            StringBuilder sb = new StringBuilder();
            using (context)
            {
                var books = context.Books
                    .Where(b => b.Author.LastName.StartsWith(input))
                    .OrderBy(b => b.BookId)
                    .Select(b => new
                    {
                        b.Title,
                        FullName = b.Author.FirstName + " " + b.Author.LastName
                    })
                    .ToList();
                foreach (var b in books)
                {
                    sb.AppendLine($"{b.Title} ({b.FullName})");
                }
            }
            return sb.ToString().TrimEnd();
        }

        //11. Count Books
        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            using (context)
            {
                var books = context.Books
                    .Where(b => b.Title.Length > lengthCheck)
                    .ToList();
                return books.Count();
            }
        }

        //12. Total Book Copies
        public static string CountCopiesByAuthor(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();
            using (context)
            {
                var books = context.Authors
                     .Select(a => new
                     {
                         FullName = a.FirstName + " " + a.LastName,
                         SumOfCopies = a.Books.Sum(b => b.Copies)
                     })
                     .OrderByDescending(a => a.SumOfCopies)
                     .ToList();

                foreach (var a in books)
                {
                    sb.AppendLine($"{a.FullName} - {a.SumOfCopies}");
                }
            }
            return sb.ToString().TrimEnd();
        }

        //13. Profit by Category
        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();
            using (context)
            {
                var categories = context.Categories
                    .Select(bc => new
                    {
                        Category = bc.Name,
                        Profit = bc.CategoryBooks.Sum(b => b.Book.Price * b.Book.Copies)
                    })
                    .OrderByDescending(bc => bc.Profit)
                    .ThenBy(bc => bc.Category)
                    .ToList();
                foreach (var c in categories)
                {
                    sb.AppendLine($"{c.Category} {c.Profit:f2}");
                }
            }
            return sb.ToString().TrimEnd();
        }

        //14. Most Recent Books
        public static string GetMostRecentBooks(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();
            using (context)
            {
                var books = context.Categories
                     .Select(c => new
                     {
                         Name = c.Name,
                         Books =
                         (context.BooksCategories
                         .Where(b => b.CategoryId == c.CategoryId)
                         .OrderByDescending(b => b.Book.ReleaseDate)
                         .Select(b => new
                         {
                             Title = b.Book.Title,
                             Year = b.Book.ReleaseDate.Value.Year
                         })
                         .Take(3)
                         .ToList())
                     })
                     .OrderBy(c => c.Name)
                     .ToList();
                foreach (var c in books)
                {
                    sb.AppendLine($"--{c.Name}");
                    foreach (var b in c.Books)
                    {
                        sb.AppendLine($"{b.Title} ({b.Year})");
                    }
                }
            }
            return sb.ToString().TrimEnd();
        }

        //15. Increase Prices
        public static void IncreasePrices(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();
            using (context)
            {
                var books = context.Books
                    .Where(b => b.ReleaseDate.Value.Year < 2010)
                    .ToList();
                foreach (var b in books)
                {
                    b.Price += 5;
                }
                context.SaveChanges();
            }
        }

        //16. Remove Books
        public static int RemoveBooks(BookShopContext context)
        {
            using (context)
            {
                var books = context
                     .Books
                     .Where(b => b.Copies < 4200);

                var delete = books.Count();

                var bookCategories = context
                    .BooksCategories
                    .Where(bc => bc.Book.Copies < 4200);

                context.BooksCategories.RemoveRange(bookCategories);

                context.Books.RemoveRange(books);

                context.SaveChanges();

                return delete;
            }
        }
    }
}
