using Microsoft.EntityFrameworkCore;
using BarangaySystem.Models;

namespace BarangaySystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        // One DbSet = one table in the database
        public DbSet<User> Users { get; set; }
        public DbSet<DocumentType> DocumentTypes { get; set; }
        public DbSet<DocumentRequest> DocumentRequests { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Pre-load 3 document types when the database is first created
            modelBuilder.Entity<DocumentType>().HasData(
                new DocumentType { Id=1, Name="Barangay Clearance", Fee=50 },
                new DocumentType { Id=2, Name="Certificate of Indigency", Fee=0 },
                new DocumentType { Id=3, Name="Certificate of Residency", Fee=50 }
            );

            // Pre-load one Admin Account
            modelBuilder.Entity<User>().HasData(new User
            {
                Id=1,
                FullName = "Barangay Admin",
                Email="admin@barangay.gov.ph",
                PasswordHash=BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                Role="Admin",
                Address="Barangay Hall",
                ContactNumber="09000000000"
            });
        }
    }
}