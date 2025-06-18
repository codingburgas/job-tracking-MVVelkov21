namespace JobTracking.Application.DTOs.Request
{
    using JobTracking.Domain.Enums;
    public class UpdateJobPostingRequest
    {
        public string Title { get; set; }
        public string CompanyName { get; set; }
        public string Description { get; set; }
        public JobPostingStatus? Status { get; set; }
    }
}