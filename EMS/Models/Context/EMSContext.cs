using Microsoft.EntityFrameworkCore;
using EMS.Models.Entity;

namespace EMS.Models.Context
{
    public class EMSContext : DbContext
    {
        public EMSContext(DbContextOptions<EMSContext> options) : base(options)
        {
        }

        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Department entity konfigürasyonu
            modelBuilder.Entity<Department>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired();
            });

            // Employee entity konfigürasyonu
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FullName).IsRequired();
                entity.Property(e => e.Email).IsRequired();
                entity.Property(e => e.Salary).HasColumnType("decimal(18,2)");

                // Department ile ilişki
                entity.HasOne(e => e.Department)
                    .WithMany(d => d.Employees)
                    .HasForeignKey(e => e.DepartmentId);
            });
        }
    }
}