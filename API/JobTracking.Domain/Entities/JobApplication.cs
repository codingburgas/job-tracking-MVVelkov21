namespace JobTracking.Domain.Entities
{
    using JobTracking.Domain.Enums;

    public class JobApplication : Base.IEntity
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid JobPostingId { get; set; }
        public ApplicationStatus Status { get; set; } = ApplicationStatus.Submitted;
        public DateTime ApplicationDate { get; set; } = DateTime.UtcNow;

        // Navigation properties (if using ORM like Entity Framework Core)
        public User User { get; set; }
        public JobPosting JobPosting { get; set; }
    }
}