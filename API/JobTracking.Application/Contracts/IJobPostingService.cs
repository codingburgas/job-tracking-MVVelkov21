namespace JobTracking.Application.Contracts
{
    using JobTracking.Application.DTOs.Request;
    using JobTracking.Application.DTOs.Response;
    using JobTracking.Domain.Filters;
    using JobTracking.Domain.Enums;

    public interface IJobPostingService
    {
        Task<JobPostingResponse> CreateJobPostingAsync(CreateJobPostingRequest request);
        Task<JobPostingResponse> UpdateJobPostingAsync(Guid id, UpdateJobPostingRequest request);
        Task<bool> DeleteJobPostingAsync(Guid id);
        Task<IEnumerable<JobPostingResponse>> GetAllJobPostingsAsync(JobPostingFilter filter);
        Task<JobPostingResponse> GetJobPostingByIdAsync(Guid id);
        Task<IEnumerable<JobPostingResponse>> GetActiveJobPostingsAsync(JobPostingFilter filter);
    }

    // JobTracking.Domain/Filters/JobPostingFilter.cs (New Filter specific to job postings)
    public class JobPostingFilter : BaseFilter
    {
        public string SearchTerm { get; set; }
        public JobPostingStatus? Status { get; set; }
    }
}