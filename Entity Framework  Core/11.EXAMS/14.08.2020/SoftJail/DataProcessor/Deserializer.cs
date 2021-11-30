namespace SoftJail.DataProcessor
{

    using Data;
    using Newtonsoft.Json;
    using SoftJail.Data.Models;
    using SoftJail.Data.Models.Enums;
    using SoftJail.DataProcessor.ImportDto;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Xml.Serialization;

    public class Deserializer
    {
        private static string InvalidData = "Invalid Data";
        private static string SuccessDepartment = "Imported {0} with {1} cells";
        private static string SuccessPriconer = "Imported {0} {1} years old";
        private static string SuccessOfficers = "Imported {0} ({1} prisoners)";
        public static string ImportDepartmentsCells(SoftJailDbContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
         
            var departmentsDto = JsonConvert.DeserializeObject<ImportDepartmentsDto[]>(jsonString);

            List<Department> departments = new List<Department>();
          

            foreach (var departDto in departmentsDto)
            {
                if (!IsValid(departDto))
                {
                    sb.AppendLine(InvalidData);
                    continue;
                }
                List<Cell> cells = new List<Cell>();
                var isValidCells = false;
                foreach (var cell in departDto.Cells)
                {
                    if (!IsValid(cell))
                    {
                        sb.AppendLine(InvalidData);
                        isValidCells = false;
                        break;
                    }
                    isValidCells = true;
                    cells.Add(new Cell()
                    {
                        CellNumber = cell.CellNumber,
                        HasWindow = cell.HasWindow
                    });
                }

                if (!isValidCells)
                {
                    continue;
                }

                var department = new Department()
                {
                    Name = departDto.Name,
                    Cells = cells
                };
                departments.Add(department);
                sb.AppendLine(string.Format(SuccessDepartment, department.Name, department.Cells.Count));
            }
            context.Departments.AddRange(departments);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportPrisonersMails(SoftJailDbContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            var prisonersDto = JsonConvert.DeserializeObject<ImportPrisonersDto[]>(jsonString);

            List<Prisoner> prisoners = new List<Prisoner>();

            foreach (var prisonerDto in prisonersDto)
            {
                if (!IsValid(prisonerDto))
                {
                    sb.AppendLine(InvalidData);
                    continue;
                }

                DateTime incarcerationDate;
                bool isValidDate = DateTime.TryParseExact(prisonerDto.IncarcerationDate, "dd/MM/yyyy",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out incarcerationDate);

                if (!isValidDate)
                {
                    sb.AppendLine(InvalidData);
                    continue;
                }

                DateTime? releaseDate = null;

                if (!String.IsNullOrWhiteSpace(prisonerDto.ReleaseDate))
                {
                    DateTime dueDateDt;
                    bool isDueDateValid = DateTime.TryParseExact(prisonerDto.ReleaseDate, "dd/MM/yyyy",
                        CultureInfo.InvariantCulture, DateTimeStyles.None, out dueDateDt);

                    if (!isDueDateValid)
                    {
                        sb.AppendLine(InvalidData);
                        continue;
                    }

                    releaseDate = dueDateDt;
                }

                var prisoner = new Prisoner()
                {
                    Nickname = prisonerDto.Nickname,
                    FullName = prisonerDto.FullName,
                    Age = prisonerDto.Age,
                    IncarcerationDate = incarcerationDate,
                    ReleaseDate = releaseDate,
                    Bail = prisonerDto.Bail,
                    CellId = prisonerDto.CellId
                };
                bool isValidMails = true;
                foreach (var mail in prisonerDto.Mails)
                {
                    if (!IsValid(mail))
                    {
                        sb.AppendLine(InvalidData);
                        isValidMails = false;
                        break;
                    }

                    prisoner.Mails.Add(new Mail()
                    { 
                        Description = mail.Description,
                        Address = mail.Address,
                        Sender = mail.Sender
                    });
                }

                if (!isValidMails)
                {
                    continue;
                }

                prisoners.Add(prisoner);
                sb.AppendLine(string.Format(SuccessPriconer, prisoner.FullName, prisoner.Age));
            }

            context.Prisoners.AddRange(prisoners);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        public static string ImportOfficersPrisoners(SoftJailDbContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportOfficersDto[]), new XmlRootAttribute("Officers"));

            using StringReader stringReader = new StringReader(xmlString);

            var officersDto = (ImportOfficersDto[])xmlSerializer.Deserialize(stringReader);

            List<Officer> officers = new List<Officer>();

            foreach (var officerDto in officersDto)
            {
                if (!IsValid(officerDto))
                {
                    sb.AppendLine(InvalidData);
                    continue;
                }

                Weapon weapon;
                var isValid = Enum.TryParse<Weapon>(officerDto.Weapon, out weapon);
                if (!isValid)
                {
                    sb.AppendLine(InvalidData);
                    continue;

                }
                Position position;
                isValid = Enum.TryParse<Position>(officerDto.Position, out position);
                if (!isValid)
                {
                    sb.AppendLine(InvalidData);
                    continue;

                }

                var officer = new Officer()
                {
                    FullName = officerDto.Name,
                    Position = position,
                    Weapon = weapon,
                    Salary = officerDto.Money,
                    DepartmentId = officerDto.DepartmentId,
                };

                List<OfficerPrisoner> officerPrisoners = new List<OfficerPrisoner>();
                foreach (var prisoner in officerDto.Prisoners)
                {
                    officerPrisoners.Add(new OfficerPrisoner()
                    {
                        PrisonerId = prisoner.PrisonerId,
                        Officer = officer
                    });
                }

                officer.OfficerPrisoners = officerPrisoners;
                officers.Add(officer);

                sb.AppendLine(string.Format(SuccessOfficers, officer.FullName, officer.OfficerPrisoners.Count));
            }
            context.Officers.AddRange(officers);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object obj)
        {
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(obj);
            var validationResult = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(obj, validationContext, validationResult, true);
            return isValid;
        }
    }
}