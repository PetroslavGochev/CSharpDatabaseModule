using Microsoft.EntityFrameworkCore;
using P01_HospitalDatabase.Data.Models;

namespace P01_HospitalDatabase.Data
{
    public class HospitalContext:DbContext
    {
        public HospitalContext()
        {

        }
        public HospitalContext(DbContextOptions options)
            :base(options)
        {

        }

        public virtual DbSet<Patient> Patients { get; set; }
        public virtual DbSet<Diagnose> Diagnoses { get; set; }
        public virtual DbSet<Medicament> Medicaments { get; set; }
        public virtual DbSet<PatientMedicament> PatientMedicaments { get; set; }
        public virtual DbSet<Visitation> Visitations { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(ConfigurationConection.Conection);
            }
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Medicament>(entity => 
            {
                entity
                .Property(m => m.Name)
                .IsUnicode(true);
            });
            modelBuilder.Entity<PatientMedicament>(entity =>
            {
                entity.HasKey(pm => new { pm.MedicamentId, pm.PatientId });
                entity
                .HasOne(pm => pm.Medicament)
                .WithMany(m => m.Prescriptions)
                .HasForeignKey(pm => pm.MedicamentId);
                entity
               .HasOne(pm => pm.Patient)
               .WithMany(m => m.Prescriptions)
               .HasForeignKey(pm => pm.PatientId);
            });
            modelBuilder.Entity<Visitation>(entity =>
            {
                entity
                .Property(v => v.Comments)
                .IsUnicode(true);
                entity
                .HasOne(v => v.Patient)
                .WithMany(p => p.Visitations)
                .HasForeignKey(v => v.PatientId);
            });
            modelBuilder.Entity<Diagnose>(entity =>
            {
                entity
                .Property(d => d.Name)
                .IsUnicode(true);

                entity
                .Property(d => d.Comments)
                .IsUnicode(true);

                entity
               .HasOne(d => d.Patient)
               .WithMany(p => p.Diagnoses)
               .HasForeignKey(d => d.PatientId);
            });
            modelBuilder.Entity<Patient>(entity =>
            {
                entity
                .Property(p => p.FirstName)
                .IsUnicode(true);
                entity
               .Property(p => p.LastName)
               .IsUnicode(true);
                entity
               .Property(p => p.Address)
               .IsUnicode(true);
                entity
               .Property(p => p.Email)
               .IsUnicode(false);
            });
        }
    }
}
