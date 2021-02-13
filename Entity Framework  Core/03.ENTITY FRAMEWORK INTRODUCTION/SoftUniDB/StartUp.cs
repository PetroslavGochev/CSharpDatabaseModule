using System;
using System.Collections.Generic;
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
                Console.WriteLine(RemoveTown(context));


            }
        }

        //3.	Employees Full Information
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

        //4.	Employees with Salary Over 50 000
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

        //5.	Employees from Research and Development
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

        //6.	Adding a New Address and Updating Employee
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
            return sb.ToString().TrimEnd();
        }

        //7.	Employees and Projects
        public static string GetEmployeesInPeriod(SoftUniContext context)
        {

            StringBuilder sb = new StringBuilder();
            using (context)
            {
                var employee = context.Employees
                    .Select(e => new
                    {
                        e.FirstName,
                        e.LastName,
                        Manager = e.Manager,
                        Project = e.EmployeesProjects
                        .Where(p => p.Project.StartDate.Year >= 2001 && p.Project.StartDate.Year <= 2003)
                        .Select(p => new
                        {
                            Name = p.Project.Name,
                            StartDate = p.Project.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture),
                            EndDate = p.Project.EndDate.HasValue ?
                        p.Project.EndDate.Value.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture) : "not finished"

                        })
                        .ToList()

                    })
                    .Take(10)
                    .ToList();

                foreach (var ep in employee)
                {
                    sb.AppendLine($"{ep.FirstName} {ep.LastName} - Manager: {ep.Manager.FirstName} {ep.Manager.LastName}");
                    foreach (var p in ep.Project)
                    {
                        sb.AppendLine($"--{p.Name} - {p.StartDate} - {p.EndDate}");
                    }
                }
            }
            return sb.ToString().TrimEnd();
        }

        //8.	Addresses by Town
        public static string GetAddressesByTown(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();
            using (context)
            {
                var employeeAddress = context.Addresses
                    .Select(e => new
                    {
                        Employees = e.Employees
                        .ToList(),
                        AddressText = e.AddressText,
                        Town = e.Town
                    })
                    .OrderByDescending(e => e.Employees.Count())
                    .ThenBy(t => t.Town.Name)
                    .ThenBy(a => a.AddressText)
                    .Take(10)
                    .ToList();

                foreach (var ea in employeeAddress)
                {
                    sb.AppendLine($"{ea.AddressText}, {ea.Town.Name} - {ea.Employees.Count()} employees");
                }
            }
            return sb.ToString().TrimEnd();
        }

        //9.	Employee 147
        public static string GetEmployee147(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();
            using (context)
            {
                var employee = context.Employees
                    .Select(e => new
                    {
                        Id = e.EmployeeId,
                        Name = e.FirstName + e.LastName,
                        JobTitle = e.JobTitle,
                        Project = e.EmployeesProjects
                        .Select(p => new
                        {
                            Project = p.Project
                        })
                        .OrderBy(p => p.Project.Name)
                        .ToList()
                    })
                    .Where(e => e.Id == 147)
                    .ToList();

                foreach (var e in employee)
                {
                    sb.AppendLine($"{e.Name} - {e.JobTitle}");
                    foreach (var p in e.Project)
                    {
                        sb.AppendLine($"{p.Project.Name}");
                    }
                }
            }
            return sb.ToString().TrimEnd();
        }

        //10.	Departments with More Than 5 Employees
        public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            using (context)
            {
                var department = context.Departments
                    .Select(d => new
                    {
                        NameOfDepartment = d.Name,
                        ManagerName = d.Manager.FirstName + d.Manager.LastName,
                        Employees = d.Employees
                        .Select(e => new
                        {
                            FirstName = e.FirstName,
                            LastName = e.LastName,
                            Jobtitle = e.JobTitle,
                        })
                        .OrderBy(e => e.FirstName)
                        .ThenBy(e => e.LastName)
                        .ToList()
                    })
                    .Where(d => d.Employees.Count() > 5)
                    .OrderBy(d => d.Employees.Count())
                    .ThenBy(d => d.NameOfDepartment)
                    .ToList();

                foreach (var d in department)
                {
                    sb.AppendLine($"{d.NameOfDepartment} - {d.ManagerName}");
                    foreach (var e in d.Employees)
                    {
                        sb.AppendLine($"{e.FirstName} {e.LastName} - {e.Jobtitle}");
                    }
                }

            }

            return sb.ToString().TrimEnd();
        }

        //11.	Find Latest 10 Projects
        public static string GetLatestProjects(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            using(context)
            {
                var projects = context.Projects
                    .Select(p => new
                    {
                        StartDate = p.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture),
                        NameOfProject = p.Name,
                        Description = p.Description
                    })               
                    .ToList();

                foreach (var p in projects.TakeLast(10).OrderBy(p => p.NameOfProject))
                {
                    sb.AppendLine($"{p.NameOfProject}");
                    sb.AppendLine($"{p.Description}");
                    sb.AppendLine($"{p.StartDate}");
                }
            }

            return sb.ToString().TrimEnd();
        }

        //12.	Increase Salaries
        public static string IncreaseSalaries(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();
            HashSet<string> department = new HashSet<string>()
            {
                "Engineering",
                "Tool Design",
                "Marketing",
                "Information Services"
            };
            using (context)
            {
                var employees = context.Employees
                    .Select(e => new
                    {
                        FirstName = e.FirstName,
                        LastName = e.LastName,
                        Salary = e.Salary * 1.12M,
                        Department = e.Department
                    })
                    .Where(d => department.Contains(d.Department.Name))
                    .OrderBy(e=> e.FirstName)
                    .ThenBy(e=>e.LastName)
                    .ToList();

                foreach (var e in employees)
                {
                    sb.AppendLine($"{e.FirstName} {e.LastName} (${e.Salary:f2})");
                }
            }


            return sb.ToString().TrimEnd();
        }

        //13.	Find Employees by First Name Starting with "Sa"
        public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();
            using (context)
            {
                var employees = context.Employees
                    .Select(e => new
                    {
                        FirstName = e.FirstName,
                        LastName = e.LastName,
                        JobTitle = e.JobTitle,
                        Salary = e.Salary
                    })
                    .Where(e => e.FirstName.StartsWith("Sa"))
                    .OrderBy(e => e.FirstName)
                    .ThenBy(e => e.LastName)
                    .ToList();

                foreach (var e in employees)
                {
                    sb.AppendLine($"{e.FirstName} {e.LastName} - {e.JobTitle} (${e.Salary:f2})");
                }
            }


            return sb.ToString().TrimEnd();
        }

        //14.	Delete Project by Id
        public static string DeleteProjectById(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            using (context)
            {
                var ep = context.EmployeesProjects
                    .Where(ep => ep.ProjectId == 2)
                    .ToList();
                var project = context.Projects
                    .Where(p => p.ProjectId == 2)
                    .FirstOrDefault();
                foreach (var p in ep)
                {
                    context.Remove<EmployeesProject>(p);
                }
                if(project != null)
                {
                    context.Remove<Project>(project);
                    context.SaveChanges();
                }
               
                var projects = context
                    .Projects
                    .Take(10)
                    .ToList();
                foreach (var p in projects)
                {
                    sb.AppendLine($"{p.Name}");
                }
            }

            return sb.ToString().TrimEnd();
        }

        //15.	Remove Town
        public static string RemoveTown(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();
            using (context)
            {
                var employees = context.Employees
                    .Where(e => e.Address.Town.Name == "Seattle")
                    .ToList();
                int countOfAddress = employees.Count();
                sb.AppendLine($"{countOfAddress} addresses in Seattle were deleted");
                while (true)
                {
                    var employee = context.Employees
                         .Where(e => e.Address.Town.Name == "Seattle")
                         .FirstOrDefault();
                    if(employee == null)
                    {
                        break;
                    }
                    employee.AddressId = null;
                    context.SaveChanges();
                }
                var addresses = context.Addresses
                    .Where(a => a.Town.Name == "Seattle")
                    .ToList();
                foreach (var a in addresses)
                {
                    context.Remove<Address>(a);
                    context.SaveChanges();
                }
                var town = context.Towns
                    .Where(t => t.Name == "Seattle")
                    .FirstOrDefault();
                context.Remove<Town>(town);
                context.SaveChanges();         
            }
            return sb.ToString().TrimEnd();
        }
    }
}
