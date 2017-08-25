using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace APIToClassDatabase.Models
{
    public partial class CoursesContext : DbContext
    {
        public virtual DbSet<ClassTracker> ClassTracker { get; set; }

        public CoursesContext(DbContextOptions<CoursesContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ClassTracker>(entity =>
            {
                entity.ToTable("class_tracker");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Course)
                    .IsRequired()
                    .HasColumnName("course");
                //.HasColumnType("nchar(10)");

                entity.Property(e => e.DateDue)
                    .IsRequired()
                    .HasColumnName("date_due");
                //.HasColumnType("nchar(10)");

                entity.Property(e => e.Importance)
                    .HasColumnName("importance");
                    //.HasColumnType("nchar(10)");

                entity.Property(e => e.Semester)
                    .IsRequired()
                    .HasColumnName("semester");
                    //.HasColumnType("nchar(10)");
            });
        }
    }
}