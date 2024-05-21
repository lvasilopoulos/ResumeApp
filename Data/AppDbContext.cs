using Microsoft.EntityFrameworkCore;
using ResumeApp.Models;

namespace ResumeApp.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Candidates> Candidates { get; set; }
        public DbSet<Degrees> Degrees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Candidates>()
                .HasKey(c => c.Id);

            modelBuilder.Entity<Candidates>()
            .Property(c => c.FirstName)
            .IsRequired()
            .HasMaxLength(50);

            modelBuilder.Entity<Candidates>()
                .Property(c => c.LastName)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<Candidates>()
            .Property(c => c.Email)
            .IsRequired();

            modelBuilder.Entity<Candidates>()
                .Property(c => c.Mobile)
                .HasMaxLength(10);

            modelBuilder.Entity<Candidates>()
                .Property(c => c.CreationTime)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<Candidates>()
                .HasOne(c => c.Degrees)               // Candidate has one Degree
                .WithMany()                          // Degree can be associated with many Candidates
                .HasForeignKey(c => c.DegreeId)    // Foreign key in Candidate referencing Degree's primary key
                .OnDelete(DeleteBehavior.SetNull); // Allow DegreeId to be null

            modelBuilder.Entity<Degrees>()
                .HasKey(d => d.Id);

            modelBuilder.Entity<Degrees>()
                .Property(d => d.Name)
                .IsRequired();

            modelBuilder.Entity<Degrees>()
            .Property(d => d.CreationTime)
            .HasDefaultValueSql("GETDATE()");
        }
    }
}
