using System;
using System.Collections.Generic;

namespace P01_StudentSystem.Data.Models
{
    public class Student
    {
        public Student()
        {
            this.HomeworkSubmissions = new HashSet<Homework>();
            this.CourseEnrollments = new HashSet<StudentCourse>();
        }
        public int StudentId { get; set; }

        public string Name { get; set; }

        public string PhoneNumber { get; set; }

        public int RegisteredOn { get; set; }

        public DateTime Birthday { get; set; }

        public virtual ICollection<Homework> HomeworkSubmissions { get; set; }

        public virtual ICollection<StudentCourse> CourseEnrollments { get; set; }
    }
}
