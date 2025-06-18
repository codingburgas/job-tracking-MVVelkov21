namespace JobTracking.Application.DTOs.Response
{
    public class JobApplicationResponse
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid JobPostingId { get; set; }
        public string Status { get; set; }
        public DateTime ApplicationDate { get; set; }
    }
}