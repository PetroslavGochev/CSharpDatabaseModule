using System;
using System.Globalization;
using System.Linq;
using System.Text;
using SoftUniDB.Data.Models;

namespace SoftUniDB
{
    public class StartUp
    {
        static void Main(string[] args)
        {
            var context = new SoftUniContext();
            using (context)
            {
                //Console.WriteLine(GetEmployeesInPeriod(context));
                
            }
        }

        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();
            using (context)
            {
                var listOfEmployees = context.Employees
                    .Select(e => new
                    {
                        e.EmployeeId,
                        e.FirstName,
                        e.LastName,
                        e.MiddleName,
                        e.JobTitle,
                        e.Salary
                    })
                    .OrderBy(e => e.EmployeeId)
                    .ToList();

                foreach (var e in listOfEmployees)
                {
                    sb.AppendLine($"{e.FirstName} {e.LastName} {e.MiddleName} {e.JobTitle} {e.Salary:f2}");
                }
            }        
            return sb.ToString().TrimEnd();
        }

        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            using (context)
            {
                var listOfemployees = context.Employees
                    .Select(e => new
                    {
                        e.FirstName,
                        e.Salary
                    })
                    .Where(x => x.Salary > 50000)
                    .OrderBy(x => x.FirstName)
                    .ToList();
                foreach (var e in listOfemployees)
                {
                    sb.AppendLine($"{e.FirstName} - {e.Salary:f2}");
                }
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();
            using (context)
            {
                var employees = context.Employees
                    .Select(x => new
                    {
                        x.FirstName,
                        x.LastName,
                        x.Department,
                        x.Salary
                    })
                    .Where(x => x.Department.Name == "Research and Development")
                    .OrderBy(e => e.Salary)
                    .ThenByDescending(e => e.FirstName)
                    .ToList();

                foreach (var e in employees)
                {
                    sb.AppendLine($"{e.FirstName} {e.LastName } from {e.Department.Name} - {e.Salary:f2}");
                }

            }

            return sb.ToString().TrimEnd();
        }
        public static string AddNewAddressToEmployee(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();
            using (context)
            {
                var address = new Address
                {
                    AddressText = "Vitoshka 15",
                    TownId = 4
                };

                context.Add<Address>(address);
                context.SaveChanges();

                var employee = context.Employees
                    .Where(x => x.LastName == "Nakov")
                    .FirstOrDefault();
                if (employee != null)
                {
                    employee.Address = context.Addresses
                        .Where(x => x.AddressText == "Vitoshka 15")
                        .FirstOrDefault();
                    context.SaveChanges();
                }

                var addressOfAllEmployees = context.Employees
                    .Select(x => new
                    {
                        x.AddressId,
                        x.Address.AddressText
                    })
                    .OrderByDescending(x => x.AddressId)
                    .Take(10)
                    .ToList();

                foreach (var add in addressOfAllEmployees)
                {
                    sb.AppendLine($"{add.AddressText}");
                }
            }            
            return sb.ToString().TrimEnd() ;
        }

        //public static string GetEmployeesInPeriod(SoftUniContext context)
        //{
           
        //    //StringBuilder sb = new StringBuilder();
        //    //using (context)
        //    //{
        //    //    var employee = context.Employees
        //    //        .Select(e => new
        //    //        {
        //    //            e.FirstName,
        //    //            e.LastName,
        //    //            Manager = e.Manager,
        //    //            Project = e.EmployeesProjects
        //    //            .Select(p => new
        //    //            {
        //    //                Name = p.Project.Name,
        //    //                StartDate = p.Project.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture),
        //    //                EndDate = p.Project.EndDate.HasValue ?
        //    //            p.Project.EndDate.Value.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture) : "not finished"

        //    //            })
        //    //            .Where(p => DateTime.Parse(p.StartDate).Year >= 2001 && DateTime.Parse(p.StartDate).Year <= 2003)
        //    //            .ToList()

        //    //        })                                                
        //    //        .Take(10)
        //    //        .ToList();

        //    //    foreach (var ep in employee)
        //    //    {
        //    //        sb.AppendLine($"{ep.FirstName} {ep.LastName} - Manager: {ep.Manager.FirstName} {ep.Manager.LastName}");
        //    //        foreach (var p in ep.Project)
        //    //        {
        //    //            sb.AppendLine($"--{p.Name} - {p.StartDate} - {p.EndDate}");
        //    //        }
        //    //    }
                

                
        //    //}
        //    //return sb.ToString().TrimEnd();
        //}
    }
}
