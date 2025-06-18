namespace JobTracking.Application.Contracts
{
    using JobTracking.Application.DTOs.Request;
    using JobTracking.Application.DTOs.Response;
    using JobTracking.Domain.Enums;

    public interface IJobApplicationService
    {
        Task<JobApplicationResponse> ApplyForJobAsync(Guid userId, ApplyForJobRequest request);
        Task<IEnumerable<JobApplicationResponse>> GetUserApplicationsAsync(Guid userId);
        Task<JobApplicationResponse> UpdateApplicationStatusAsync(Guid applicationId, UpdateApplicationStatusRequest request);
        Task<IEnumerable<JobApplicationResponse>> GetAllApplicationsForJobPostingAsync(Guid jobPostingId); // Admin view
        Task<JobApplicationResponse> GetApplicationByIdAsync(Guid applicationId);
    }
}