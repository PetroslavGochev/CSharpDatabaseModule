using _01.Student_System.Data.Configuration;
using _01.Student_System.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace _01.Student_System.Data
{
    public class StudentSystemConctext : DbContext
    {
        public StudentSystemConctext()
        {

        }
        public StudentSystemConctext(DbContextOptions options)
            : base(options)
        {

        }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Homework> HomeworkSubmissions { get; set; }
        public DbSet<Recource> Recources { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<StudentCourse> StudentCourses { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if(!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                     .UseSqlServer(ConnectionConfiguration.CONNECTION);
            }
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
