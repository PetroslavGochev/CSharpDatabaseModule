namespace _01.Student_System.Data.Models
{
    using Student_System.Data.Enumerator;
    public class Recource
    {
        public int RecourceId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public ResourceTypes ResourceType  { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }
    }
}
