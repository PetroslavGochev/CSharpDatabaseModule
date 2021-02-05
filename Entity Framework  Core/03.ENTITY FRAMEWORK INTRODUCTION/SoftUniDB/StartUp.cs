using System;
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
                Console.WriteLine(GetEmployeesFromResearchAndDevelopment(context));
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
    }
}
