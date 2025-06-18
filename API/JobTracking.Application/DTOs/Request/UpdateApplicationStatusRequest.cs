namespace JobTracking.Application.DTOs.Request
{
    using JobTracking.Domain.Enums;
    public class UpdateApplicationStatusRequest
    {
        public ApplicationStatus NewStatus { get; set; }
    }
}