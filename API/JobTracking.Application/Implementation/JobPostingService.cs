namespace JobTracking.Application.Implementation
{
    using JobTracking.Application.Contracts;
    using JobTracking.Application.DTOs.Request;
    using JobTracking.Application.DTOs.Response;
    using JobTracking.DataAccess.Repositories;
    using JobTracking.Domain.Entities;
    using JobTracking.Domain.Enums;
    using JobTracking.Domain.Filters;

    public class JobPostingService : IJobPostingService
    {
        private readonly IRepository<JobPosting> _jobPostingRepository;

        public JobPostingService(IRepository<JobPosting> jobPostingRepository)
        {
            _jobPostingRepository = jobPostingRepository;
        }

        public async Task<JobPostingResponse> CreateJobPostingAsync(CreateJobPostingRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Title) || string.IsNullOrWhiteSpace(request.CompanyName) || string.IsNullOrWhiteSpace(request.Description))
            {
                throw new ArgumentException("Title, company name, and description are required.");
            }

            var jobPosting = new JobPosting
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                CompanyName = request.CompanyName,
                Description = request.Description,
                PublicationDate = DateTime.UtcNow,
                Status = JobPostingStatus.Active
            };

            await _jobPostingRepository.AddAsync(jobPosting);
            await _jobPostingRepository.SaveChangesAsync();

            return new JobPostingResponse
            {
                Id = jobPosting.Id,
                Title = jobPosting.Title,
                CompanyName = jobPosting.CompanyName,
                Description = jobPosting.Description,
                PublicationDate = jobPosting.PublicationDate,
                Status = jobPosting.Status.ToString()
            };
        }

        public async Task<JobPostingResponse> UpdateJobPostingAsync(Guid id, UpdateJobPostingRequest request)
        {
            var jobPosting = await _jobPostingRepository.GetByIdAsync(id);
            if (jobPosting == null)
            {
                throw new KeyNotFoundException($"Job posting with ID {id} not found.");
            }
            
            if (!string.IsNullOrWhiteSpace(request.Title)) jobPosting.Title = request.Title;
            if (!string.IsNullOrWhiteSpace(request.CompanyName)) jobPosting.CompanyName = request.CompanyName;
            if (!string.IsNullOrWhiteSpace(request.Description)) jobPosting.Description = request.Description;
            if (request.Status.HasValue) jobPosting.Status = request.Status.Value;

            _jobPostingRepository.Update(jobPosting);
            await _jobPostingRepository.SaveChangesAsync();

            return new JobPostingResponse
            {
                Id = jobPosting.Id,
                Title = jobPosting.Title,
                CompanyName = jobPosting.CompanyName,
                Description = jobPosting.Description,
                PublicationDate = jobPosting.PublicationDate,
                Status = jobPosting.Status.ToString()
            };
        }

        public async Task<bool> DeleteJobPostingAsync(Guid id)
        {
            var jobPosting = await _jobPostingRepository.GetByIdAsync(id);
            if (jobPosting == null)
            {
                return false;
            }

            _jobPostingRepository.Remove(jobPosting);
            await _jobPostingRepository.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<JobPostingResponse>> GetAllJobPostingsAsync(JobPostingFilter filter)
        {
            var query = await _jobPostingRepository.GetAllAsync();

            if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
            {
                query = query.Where(jp => jp.Title.Contains(filter.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                                           jp.CompanyName.Contains(filter.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                                           jp.Description.Contains(filter.SearchTerm, StringComparison.OrdinalIgnoreCase));
            }

            if (filter.Status.HasValue)
            {
                query = query.Where(jp => jp.Status == filter.Status.Value);
            }
            
            query = query.Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize);

            return query.Select(jp => new JobPostingResponse
            {
                Id = jp.Id,
                Title = jp.Title,
                CompanyName = jp.CompanyName,
                Description = jp.Description,
                PublicationDate = jp.PublicationDate,
                Status = jp.Status.ToString()
            }).ToList();
        }

        public async Task<JobPostingResponse> GetJobPostingByIdAsync(Guid id)
        {
            var jobPosting = await _jobPostingRepository.GetByIdAsync(id);
            if (jobPosting == null)
            {
                return null;
            }
            return new JobPostingResponse
            {
                Id = jobPosting.Id,
                Title = jobPosting.Title,
                CompanyName = jobPosting.CompanyName,
                Description = jobPosting.Description,
                PublicationDate = jobPosting.PublicationDate,
                Status = jobPosting.Status.ToString()
            };
        }

        public async Task<IEnumerable<JobPostingResponse>> GetActiveJobPostingsAsync(JobPostingFilter filter)
        {
            filter.Status = JobPostingStatus.Active;
            return await GetAllJobPostingsAsync(filter);
        }
    }
}