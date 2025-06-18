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

            // Configure User entity
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Username).IsUnique(); // Username must be unique
                entity.Property(e => e.Role)
                      .HasConversion<string>() // Store enum as string
                      .HasMaxLength(10); // Adjust length as needed
            });

            // Configure JobPosting entity
            modelBuilder.Entity<JobPosting>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Status)
                      .HasConversion<string>() // Store enum as string
                      .HasMaxLength(10); // Adjust length as needed
            });

            // Configure JobApplication entity
            modelBuilder.Entity<JobApplication>(entity =>
            {
                entity.HasKey(e => e.Id);
                // Composite unique index to ensure a user can only apply once to a specific job
                entity.HasIndex(e => new { e.UserId, e.JobPostingId }).IsUnique();
                entity.Property(e => e.Status)
                      .HasConversion<string>() // Store enum as string
                      .HasMaxLength(20); // Adjust length as needed

                // Define relationships
                entity.HasOne(ja => ja.User)
                      .WithMany()
                      .HasForeignKey(ja => ja.UserId)
                      .OnDelete(DeleteBehavior.Restrict); // Prevent cascading delete on user deletion

                entity.HasOne(ja => ja.JobPosting)
                      .WithMany()
                      .HasForeignKey(ja => ja.JobPostingId)
                      .OnDelete(DeleteBehavior.Cascade); // Delete applications if job posting is deleted
            });
        }
    }
}