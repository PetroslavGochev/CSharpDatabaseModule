using Microsoft.EntityFrameworkCore;
using RealEstates.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstates.Data
{
    public class RealEstateContext : DbContext
    {
        public RealEstateContext()
        {

        }
        public RealEstateContext(DbContextOptions<RealEstateContext> options)
            : base(options)
        {

        }

        public virtual DbSet<RealEstateProperty> RealEstateProperties { get; set; }
        public virtual DbSet<District> Districts { get; set; }
        public virtual DbSet<PropertyType> PropertyTypes { get; set; }
        public virtual DbSet<BuildingType> BuildingTypes { get; set; }
        public virtual DbSet<RealEstatePropertyTag> RealEstatePropertyTags { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=.;Database=RealEstate;Integrated Security=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RealEstateProperty>(entity =>
            {
                entity.HasOne(rep => rep.District)
                .WithMany(d => d.Properties)
                .HasForeignKey(rep => rep.DistrictId)
                .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(rep => rep.PropertyType)
                .WithMany(pt => pt.Properties)
                .HasForeignKey(rep => rep.PropertyTypeId);

                entity.HasOne(rep => rep.BuildingType)
                .WithMany(bt => bt.Properties)
                .HasForeignKey(rep => rep.BuildingTypeId);
            });

            modelBuilder.Entity<RealEstatePropertyTag>(entity =>
            {
                entity.HasKey(rept => new { rept.PropertyId, rept.PropertyTagId });
            });


        }
    }
}
