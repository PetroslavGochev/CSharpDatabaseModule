using MiniORM.App.Data.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MiniORM.App.Data.Entity
{
    public class Department
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public ICollection<Employee> Employees { get;  }

    }
}
