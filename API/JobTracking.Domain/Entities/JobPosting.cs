namespace JobTracking.Domain.Entities
{
    using JobTracking.Domain.Enums;

    public class JobPosting : Base.IEntity
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string CompanyName { get; set; }
        public string Description { get; set; }
        public DateTime PublicationDate { get; set; } = DateTime.UtcNow;
        public JobPostingStatus Status { get; set; } = JobPostingStatus.Active;
    }
}