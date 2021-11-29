namespace VaporStore.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Data;
    using Newtonsoft.Json;
    using VaporStore.Data.Models;
    using VaporStore.Data.Models.Enums;
    using VaporStore.DataProcessor.Dto.Import;

    public static class Deserializer
    {
        private static string InvalidData = "Invalid data";
        private static string ImportGamesCorrectly = "Added {0} ({1}) with {2} tags";
        private static string ImportUsersCorrectly = "Imported {0} with {1} cards";
        private static string ImportPurchasesCorrectly = "Imported {0} for {1}";
        public static string ImportGames(VaporStoreDbContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            ImportJsonGameDTO[] gameDtos = JsonConvert.DeserializeObject<ImportJsonGameDTO[]>(jsonString);

            List<Game> games = new List<Game>();
            List<Developer> developers = new List<Developer>();
            List<Genre> genres = new List<Genre>();
            List<Tag> tags = new List<Tag>();

            foreach (ImportJsonGameDTO gameDto in gameDtos)
            {
                if (!IsValid(gameDto))
                {
                    sb.AppendLine(InvalidData);
                    continue;
                }

                DateTime releaseDate;
                bool isReleaseDateValid = DateTime.TryParseExact(gameDto.ReleaseDate, "yyyy-MM-dd",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out releaseDate);

                if (!isReleaseDateValid)
                {
                    sb.AppendLine(InvalidData);
                    continue;
                }

                if (gameDto.Tags.Length == 0)
                {
                    sb.AppendLine(InvalidData);
                    continue;
                }

                Game g = new Game()
                {
                    Name = gameDto.Name,
                    Price = gameDto.Price,
                    ReleaseDate = releaseDate
                };

                Developer gameDev = developers
                    .FirstOrDefault(d => d.Name == gameDto.Developer);

                if (gameDev == null)
                {
                    Developer newGameDev = new Developer()
                    {
                        Name = gameDto.Developer
                    };
                    developers.Add(newGameDev);

                    g.Developer = newGameDev;
                }
                else
                {
                    g.Developer = gameDev;
                }

                Genre gameGenre = genres
                    .FirstOrDefault(g => g.Name == gameDto.Genre);

                if (gameGenre == null)
                {
                    Genre newGenre = new Genre()
                    {
                        Name = gameDto.Genre
                    };

                    genres.Add(newGenre);
                    g.Genre = newGenre;
                }
                else
                {
                    g.Genre = gameGenre;
                }

                foreach (string tagName in gameDto.Tags)
                {
                    if (String.IsNullOrEmpty(tagName))
                    {
                        continue;
                    }

                    Tag gameTag = tags
                        .FirstOrDefault(t => t.Name == tagName);

                    if (gameTag == null)
                    {
                        Tag newGameTag = new Tag()
                        {
                            Name = tagName
                        };

                        tags.Add(newGameTag);
                        g.GameTags.Add(new GameTag()
                        {
                            Game = g,
                            Tag = newGameTag
                        });
                    }
                    else
                    {
                        g.GameTags.Add(new GameTag()
                        {
                            Game = g,
                            Tag = gameTag
                        });
                    }
                }

                if (g.GameTags.Count == 0)
                {
                    sb.AppendLine(InvalidData);
                    continue;
                }

                games.Add(g);
                sb.AppendLine(String.Format(ImportGamesCorrectly, g.Name, g.Genre.Name, g.GameTags.Count));
            }

            context.Games.AddRange(games);
            context.SaveChanges();

            return sb.ToString().TrimEnd();

        }

        public static string ImportUsers(VaporStoreDbContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            var usersDto = JsonConvert.DeserializeObject<ImportJsonUserDTO[]>(jsonString);

            List<User> users = new List<User>();

            foreach (var userDto in usersDto)
            {
                if (!IsValid(userDto))
                {
                    sb.AppendLine(InvalidData);
                    continue;
                }

                var user = new User()
                {
                    FullName = userDto.FullName,
                    Username = userDto.Username,
                    Email = userDto.Email,
                    Age = userDto.Age
                };

                if(userDto.Cards.Any(c => !IsValid(c)))
                {
                    sb.AppendLine(InvalidData);
                    continue;
                }

                ICollection<Card> cards = new List<Card>();
                bool isCardTypeValid = true;
                foreach (var cardDto in userDto.Cards)
                {
                    CardType cardType;
                    var isValidType = Enum.TryParse<CardType>(cardDto.Type, out cardType);
                    if (!isValidType)
                    {
                        isCardTypeValid = false;
                        break;
                    }
                    var card = new Card()
                    {
                        Number = cardDto.Number,
                        Cvc = cardDto.Cvc,
                        Type = cardType
                    };
                    cards.Add(card);
                }

                if (!isCardTypeValid)
                {
                    sb.AppendLine(InvalidData);
                    continue;
                }

                user.Cards = cards.ToList();
                users.Add(user);
                sb.AppendLine(string.Format(ImportUsersCorrectly, user.Username, user.Cards.Count));
            }

            context.Users.AddRange(users);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportPurchases(VaporStoreDbContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportXmlPurchasesDto[]), new XmlRootAttribute("Purchases"));

            using StringReader stringReader = new StringReader(xmlString);

            ImportXmlPurchasesDto[] purchasesDto = (ImportXmlPurchasesDto[])xmlSerializer.Deserialize(stringReader);

            var purchases = new List<Purchase>();

            foreach (var purchaseDto in purchasesDto)
            {
                if (!IsValid(purchaseDto))
                {
                    sb.AppendLine(InvalidData);
                    continue;
                }

                DateTime dateTime;

                bool isDueDateValid = DateTime.TryParseExact(purchaseDto.Date, "dd/MM/yyyy HH:mm",
                        CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime);

                if (!isDueDateValid)
                {
                    sb.AppendLine(InvalidData);
                    continue;
                }
                var cardExist = context.Cards.FirstOrDefault(c => c.Number == purchaseDto.Card);

                if(cardExist == null)
                {
                    sb.AppendLine(InvalidData);
                    continue;
                }

                var gameExist = context.Games.FirstOrDefault(c => c.Name == purchaseDto.Title);

                if(gameExist == null)
                {
                    sb.AppendLine(InvalidData);
                    continue;
                }

                PurchaseType type;

                var isValidType = Enum.TryParse<PurchaseType>(purchaseDto.Type, out type);

                if (!isValidType)
                {
                    sb.AppendLine(InvalidData);
                    continue;
                }

                var purchase = new Purchase()
                {
                    ProductKey = purchaseDto.Key,
                    Type = type,
                    Game = gameExist,
                    Card = cardExist,
                    Date = dateTime
                };

                purchases.Add(purchase);
                sb.AppendLine(string.Format(ImportPurchasesCorrectly, gameExist.Name, cardExist.User.Username));
            }

            context.Purchases.AddRange(purchases);
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