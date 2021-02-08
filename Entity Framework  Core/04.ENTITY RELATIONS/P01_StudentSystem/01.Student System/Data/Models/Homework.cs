using _01.Student_System.Data.Enumerator;
using System;

namespace _01.Student_System.Data.Models
{
    public class Homework
    {

        public int HomeworkId { get; set; }
        public string Content { get; set; }
        public ContentType ContenType { get; set; }
        public TimeSpan SubmmisionTime { get; set; }

        public int StudentId { get; set; }
        public Student Student { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }
    }
}
