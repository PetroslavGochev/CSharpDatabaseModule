using _01.Student_System.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _01.Student_System.Data.Configuration
{
    public class HomeworkConfiguration : IEntityTypeConfiguration<Homework>
    {
        public void Configure(EntityTypeBuilder<Homework> builder)
        {
            builder.HasKey(h => h.HomeworkId);

            builder.Property(h => h.Content)
                .IsRequired(true)
                .IsUnicode(false);

            builder.Property(h => h.ContenType)
                .IsRequired(true);

            builder.Property(h => h.SubmmisionTime)
                .IsRequired(true);

      
        }
    }
}
