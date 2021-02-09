using P01_StudentSystem.Data.Enumerators;

namespace P01_StudentSystem.Data.Models
{
    public class Recourse
    {
        public int ResourceId { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }

        public ResourceTypes ResourceTypes { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }
    }
}
