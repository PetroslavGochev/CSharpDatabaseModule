namespace Theatre.DataProcessor
{
    using Newtonsoft.Json;
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Theatre.Data;
    using Theatre.DataProcessor.ExportDto;

    public class Serializer
    {
        public static string ExportTheatres(TheatreContext context, int numbersOfHalls)
        {
            var theatres = context.Theatres.ToArray().Where(t => t.NumberOfHalls >= numbersOfHalls && t.Tickets.Count >= 20)
                .Select(t => new ExportTheatresDto()
                {
                    Name = t.Name,
                    Halls = t.NumberOfHalls,
                    Tickets = t.Tickets.ToArray().Where(t => t.RowNumber >= 1 && t.RowNumber <= 5).Select(ti => new ExportTicketsDto()
                    {
                        Price = ti.Price,
                        RowNumber = ti.RowNumber
                    }).OrderByDescending(ti => ti.Price).ToArray(),
                    TotalIncome = t.Tickets.Where(t => t.RowNumber >= 1 && t.RowNumber <= 5).Sum(t => t.Price)
                }).OrderByDescending(t => t.Halls).ThenBy(t => t.Name).ToArray();

            return JsonConvert.SerializeObject(theatres, Formatting.Indented);
        }

        public static string ExportPlays(TheatreContext context, double rating)
        {
            StringBuilder sb = new StringBuilder();

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ExportPlaysDto[]), new XmlRootAttribute("Plays"));

            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            using StringWriter sw = new StringWriter(sb);

            var plays = context.Plays.ToArray().Where(p => p.Rating <= rating).Select(p => new ExportPlaysDto()
            {
                Title = p.Title,
                Duration = p.Duration.ToString("c"),
                Rating = p.Rating == 0 ? "Premier" : p.Rating.ToString(),
                Genre = p.Genre.ToString(),
                Actors = p.Casts.ToArray().Where(c => c.IsMainCharacter).Select(c => new ExportActorsDto()
                {
                    FullName = c.FullName,
                    MainCharacter = $"Plays main character in '{p.Title}'."
                }).OrderByDescending(a => a.FullName).ToArray()
            }).OrderBy(p => p.Title).ThenByDescending(p => p.Genre).ToArray();

              xmlSerializer.Serialize(sw, plays, namespaces);

            return sb.ToString().TrimEnd();
        }
    }
}
