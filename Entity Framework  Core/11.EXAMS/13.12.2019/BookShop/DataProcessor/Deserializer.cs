namespace BookShop.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using BookShop.Data.Models;
    using BookShop.Data.Models.Enums;
    using BookShop.DataProcessor.ImportDto;
    using Data;
    using Newtonsoft.Json;
    using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedBook
            = "Successfully imported book {0} for {1:F2}.";

        private const string SuccessfullyImportedAuthor
            = "Successfully imported author - {0} with {1} books.";

        public static string ImportBooks(BookShopContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportBookDto[]), new XmlRootAttribute("Books"));

            using StringReader stringReader = new StringReader(xmlString);

            var booksDto = (ImportBookDto[])xmlSerializer.Deserialize(stringReader);

            List<Book> books = new List<Book>();


            foreach (var bk in booksDto)
            {
                if (!IsValid(bk))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                DateTime pulishedOn;
                bool isDueDateValid = DateTime.TryParseExact(bk.PublishedOn, "MM/dd/yyyy",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out pulishedOn);

                if (!isDueDateValid)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                Genre genre = Genre.Biography;
                if(bk.Genre == 1)
                {
                    genre = Genre.Biography;
                }
                else if (bk.Genre == 2)
                {
                    genre = Genre.Business;
                }
                else if (bk.Genre == 3)
                {
                    genre = Genre.Science;
                }

                var book = new Book()
                {
                    Name = bk.Name,
                    PublishedOn = pulishedOn,
                    Genre = genre,
                    Pages = bk.Pages,
                    Price = bk.Price
                };

                books.Add(book);
                sb.AppendLine(string.Format(SuccessfullyImportedBook, book.Name, book.Price));
            }

            context.Books.AddRange(books);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        public static string ImportAuthors(BookShopContext context, string jsonString)
        {
            var authorsDto = JsonConvert.DeserializeObject<ImportAuthorsDto[]>(jsonString);

            StringBuilder sb = new StringBuilder(); 

            List<Author> authors = new List<Author>();

            foreach (var authorDto in authorsDto)
            {
                if (!IsValid(authorDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                if(context.Authors.Any(a => a.Email == authorDto.Email))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var author = new Author()
                {
                    FirstName = authorDto.FirstName,
                    LastName = authorDto.LastName,
                    Email = authorDto.Email,
                    Phone = authorDto.Phone
                };

                foreach (var bookId in authorDto.Books)
                {
                    var book = context.Books.FirstOrDefault(b => b.Id == bookId.Id);
                    if(book == null)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    var authorsBooks = new AuthorBook()
                    {
                        Author = author,
                        Book = book
                    };
                    author.AuthorsBooks.Add(authorsBooks);
                }

                if (author.AuthorsBooks.Count == 0)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                sb.AppendLine(string.Format(SuccessfullyImportedAuthor, $"{author.FirstName} {author.LastName}", author.AuthorsBooks.Count));
                authors.Add(author);
            }
            context.Authors.AddRange(authors);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}