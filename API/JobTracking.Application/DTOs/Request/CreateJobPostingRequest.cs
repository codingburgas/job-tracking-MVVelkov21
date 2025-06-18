namespace JobTracking.Application.DTOs.Request
{
    public class CreateJobPostingRequest
    {
        public string Title { get; set; }
        public string CompanyName { get; set; }
        public string Description { get; set; }
    }
}