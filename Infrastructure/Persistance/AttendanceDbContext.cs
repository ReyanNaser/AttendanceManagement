using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;

namespace Infrastructure.Persistance
{
    public class AttendanceDbContext: DbContext, Application.Common.Interfaces.IAttendanceDbContext
    {
        public AttendanceDbContext(DbContextOptions<AttendanceDbContext> options) : base(options)
        {
        }

        public DbSet<LeaveRequest> LeaveRequests { get; set; }
        public DbSet<WorkFromHome> WorkFromHomes { get; set; }
        public DbSet<AttendanceRecord> AttendanceRecords { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("uuid-ossp");
            modelBuilder.HasPostgresExtension("pgcrypto");

            // Apply generic CommonEntityConfiguration for all entities that inherit Common
            modelBuilder.ApplyConfiguration(new CommonEntityConfiguration<Employee>());
            modelBuilder.ApplyConfiguration(new CommonEntityConfiguration<AttendanceRecord>());
            modelBuilder.ApplyConfiguration(new CommonEntityConfiguration<LeaveRequest>());
            modelBuilder.ApplyConfiguration(new CommonEntityConfiguration<WorkFromHome>());
            modelBuilder.ApplyConfiguration(new CommonEntityConfiguration<Notification>());
            base.OnModelCreating(modelBuilder);
        }
    }
}
