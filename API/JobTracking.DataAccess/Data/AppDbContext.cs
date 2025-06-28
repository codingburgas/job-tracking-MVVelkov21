namespace JobTracking.DataAccess.Data
{
    using Microsoft.EntityFrameworkCore;
    using JobTracking.Domain.Entities;
    using JobTracking.Domain.Enums;

    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<JobPosting> JobPostings { get; set; }
        public DbSet<JobApplication> JobApplications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Username).IsUnique();
                entity.Property(e => e.Role)
                      .HasConversion<string>()
                      .HasMaxLength(10);
            });

            // Configure JobPosting entity
            modelBuilder.Entity<JobPosting>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Status)
                      .HasConversion<string>()
                      .HasMaxLength(10);
            });
            
            modelBuilder.Entity<JobApplication>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => new { e.UserId, e.JobPostingId }).IsUnique();
                entity.Property(e => e.Status)
                      .HasConversion<string>()
                      .HasMaxLength(20);
                
                entity.HasOne(ja => ja.User)
                      .WithMany()
                      .HasForeignKey(ja => ja.UserId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(ja => ja.JobPosting)
                      .WithMany()
                      .HasForeignKey(ja => ja.JobPostingId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}