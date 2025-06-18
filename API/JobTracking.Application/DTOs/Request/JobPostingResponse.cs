namespace JobTracking.Application.DTOs.Response
{
    public class JobPostingResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string CompanyName { get; set; }
        public string Description { get; set; }
        public DateTime PublicationDate { get; set; }
        public string Status { get; set; }
    }
}