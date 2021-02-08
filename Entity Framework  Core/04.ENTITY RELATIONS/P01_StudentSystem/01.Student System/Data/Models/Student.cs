using System;

namespace _01.Student_System.Data.Models
{
    public class Student
    {
        public int StudentId { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public int RegisterOn { get; set; }

        public DateTime Birthday { get; set; }
    }
}
