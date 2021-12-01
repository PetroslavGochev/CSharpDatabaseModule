﻿namespace BookShop.DataProcessor
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using BookShop.Data.Models.Enums;
    using BookShop.DataProcessor.ExportDto;
    using Data;
    using Newtonsoft.Json;
    using Formatting = Newtonsoft.Json.Formatting;

    public class Serializer
    {
        public static string ExportMostCraziestAuthors(BookShopContext context)
        {
            var authors = context.Authors.ToArray()
                .Select(a => new ExportAuthorsDto
                {
                    AuthorName = a.FirstName + ' ' + a.LastName,
                    Books = a.AuthorsBooks.OrderByDescending(a => a.Book.Price).Select(ab => new ExportAuthorsBooksDto()
                    {
                        BookName = ab.Book.Name,
                        BookPrice = ab.Book.Price.ToString("0.00")
                    }).ToArray()
                })
                .OrderByDescending(a => a.Books.Length)
                .ThenBy(a => a.AuthorName)
                .ToArray();

            return JsonConvert.SerializeObject(authors, Formatting.Indented);
        }

        public static string ExportOldestBooks(BookShopContext context, DateTime date)
        {
            StringBuilder sb = new StringBuilder();

            var books = context
                .Books
                .Where(b => b.PublishedOn < date && b.Genre == Genre.Science)
                .ToArray()
                .OrderByDescending(b => b.Pages)
                .ThenByDescending(b => b.PublishedOn)
                .Take(10)
                .Select(b => new ExportBookDto()
                {
                    Name = b.Name,
                    Pages = b.Pages,
                    PublishedOn = b.PublishedOn.ToString("d", CultureInfo.InvariantCulture)
                })
                .ToArray();

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ExportBookDto[]), new XmlRootAttribute("Books"));

            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(String.Empty, String.Empty);

            xmlSerializer.Serialize(new StringWriter(sb), books, namespaces);

            return sb.ToString().TrimEnd();
        }
    }
}