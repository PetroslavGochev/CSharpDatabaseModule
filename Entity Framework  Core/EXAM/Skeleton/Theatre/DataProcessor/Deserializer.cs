namespace Theatre.DataProcessor
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Theatre.Data;
    using Theatre.Data.Models;
    using Theatre.Data.Models.Enums;
    using Theatre.DataProcessor.ImportDto;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfulImportPlay
            = "Successfully imported {0} with genre {1} and a rating of {2}!";

        private const string SuccessfulImportActor
            = "Successfully imported actor {0} as a {1} character!";

        private const string SuccessfulImportTheatre
            = "Successfully imported theatre {0} with #{1} tickets!";

        public static string ImportPlays(TheatreContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportPlaysDto[]), new XmlRootAttribute("Plays"));
            using StringReader stringReader = new StringReader(xmlString);
            var playsDtos = (ImportPlaysDto[])xmlSerializer.Deserialize(stringReader);

            List<Play> plays = new List<Play>();
            foreach (var playDto in playsDtos)
            {
                if (!IsValid(playDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                TimeSpan duration;
                bool isTimeSpanValid = TimeSpan.TryParseExact(playDto.Duration, "c", CultureInfo.InvariantCulture, TimeSpanStyles.None, out duration);
                if (!isTimeSpanValid)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                if (duration.TotalHours < 1)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Genre genre;
                bool isValidGerne = Enum.TryParse<Genre>(playDto.Genre, out genre);
                if (!isValidGerne)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var play = new Play()
                {
                    Rating = playDto.Rating,
                    Screenwriter = playDto.Screenwriter,
                    Title = playDto.Title,
                    Duration = duration,
                    Genre = genre,
                    Description = playDto.Description
                };
                sb.AppendLine(string.Format(SuccessfulImportPlay, play.Title, play.Genre, play.Rating));
                plays.Add(play);
            }
            context.AddRange(plays);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        public static string ImportCasts(TheatreContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportCastsDto[]), new XmlRootAttribute("Casts"));
            using StringReader stringReader = new StringReader(xmlString);
            var castsDtos = (ImportCastsDto[])xmlSerializer.Deserialize(stringReader);

            List<Cast> casts = new List<Cast>();
            foreach (var castDto in castsDtos)
            {
                if (!IsValid(castDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var play = context.Plays.Find(castDto.PlayId);

                Cast cast = new Cast()
                {
                    FullName = castDto.FullName,
                    IsMainCharacter = castDto.IsMainCharacter,
                    PhoneNumber = castDto.PhoneNumber,
                    PlayId = castDto.PlayId,
                    Play = play
                };

                var character = cast.IsMainCharacter ? "main" : "lesser";

                sb.AppendLine(string.Format(SuccessfulImportActor, cast.FullName, character));
                casts.Add(cast);
            }
            context.Casts.AddRange(casts);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportTtheatersTickets(TheatreContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            var importProjections = JsonConvert.DeserializeObject<ImportProjectionsDto[]>(jsonString);

            List<Theatre> theatres = new List<Theatre>();

            foreach (var projectionsDto in importProjections)
            {
                if (!IsValid(projectionsDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var theatre = new Theatre()
                {
                    Name = projectionsDto.Name,
                    NumberOfHalls = projectionsDto.NumberOfHalls,
                    Director = projectionsDto.Director
                };

                List<Ticket> tickets = new List<Ticket>();
                var invalidData = false;
                foreach (var ticketsDto in projectionsDto.Tickets)
                {
                    if (!IsValid(ticketsDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    var ticket = new Ticket()
                    {
                        Price = ticketsDto.Price,
                        RowNumber = ticketsDto.RowNumber,
                        PlayId = ticketsDto.PlayId
                        
                    };

                    tickets.Add(ticket);
                }

                if (invalidData)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                theatre.Tickets.AddRange(tickets);
                theatres.Add(theatre);
                sb.AppendLine(string.Format(SuccessfulImportTheatre, theatre.Name, theatre.Tickets.Count));
            }

            context.Theatres.AddRange(theatres);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }


        private static bool IsValid(object obj)
        {
            var validator = new ValidationContext(obj);
            var validationRes = new List<ValidationResult>();

            var result = Validator.TryValidateObject(obj, validator, validationRes, true);
            return result;
        }
    }
}
