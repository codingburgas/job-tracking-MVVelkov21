namespace JobTracking.Application.Implementation
{
    using JobTracking.Application.Contracts;
    using JobTracking.Application.DTOs.Request;
    using JobTracking.Application.DTOs.Response;
    using JobTracking.DataAccess.Repositories;
    using JobTracking.Domain.Entities;
    using JobTracking.Domain.Enums;
    using System.Linq;

    public class JobApplicationService : IJobApplicationService
    {
        private readonly IRepository<JobApplication> _applicationRepository;
        private readonly IRepository<JobPosting> _jobPostingRepository;
        private readonly IRepository<User> _userRepository;

        public JobApplicationService(
            IRepository<JobApplication> applicationRepository,
            IRepository<JobPosting> jobPostingRepository,
            IRepository<User> userRepository)
        {
            _applicationRepository = applicationRepository;
            _jobPostingRepository = jobPostingRepository;
            _userRepository = userRepository;
        }

        public async Task<JobApplicationResponse> ApplyForJobAsync(Guid userId, ApplyForJobRequest request)
        {
            // Validate JobPosting exists and is active
            var jobPosting = await _jobPostingRepository.GetByIdAsync(request.JobPostingId);
            if (jobPosting == null || jobPosting.Status != JobPostingStatus.Active)
            {
                throw new InvalidOperationException("Job posting not found or not active.");
            }

            // Validate User exists
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null || user.Role == UserRole.Admin) // Admins cannot apply
            {
                throw new UnauthorizedAccessException("User not found or not authorized to apply.");
            }

            // Check if user has already applied for this specific job posting
            var existingApplication = (await _applicationRepository.FindAsync(
                ja => ja.UserId == userId && ja.JobPostingId == request.JobPostingId
            )).FirstOrDefault();

            if (existingApplication != null)
            {
                throw new InvalidOperationException("You have already applied for this job posting.");
            }

            var application = new JobApplication
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                JobPostingId = request.JobPostingId,
                Status = ApplicationStatus.Submitted, // Default status
                ApplicationDate = DateTime.UtcNow
            };

            await _applicationRepository.AddAsync(application);
            await _applicationRepository.SaveChangesAsync();

            return new JobApplicationResponse
            {
                Id = application.Id,
                UserId = application.UserId,
                JobPostingId = application.JobPostingId,
                Status = application.Status.ToString(),
                ApplicationDate = application.ApplicationDate
            };
        }

        public async Task<IEnumerable<JobApplicationResponse>> GetUserApplicationsAsync(Guid userId)
        {
            var applications = await _applicationRepository.FindAsync(ja => ja.UserId == userId);

            return applications.Select(ja => new JobApplicationResponse
            {
                Id = ja.Id,
                UserId = ja.UserId,
                JobPostingId = ja.JobPostingId,
                Status = ja.Status.ToString(),
                ApplicationDate = ja.ApplicationDate
            }).ToList();
        }

        public async Task<JobApplicationResponse> UpdateApplicationStatusAsync(Guid applicationId, UpdateApplicationStatusRequest request)
        {
            var application = await _applicationRepository.GetByIdAsync(applicationId);
            if (application == null)
            {
                throw new KeyNotFoundException($"Application with ID {applicationId} not found.");
            }

            // Ensure the requested status is a valid enum value
            if (!Enum.IsDefined(typeof(ApplicationStatus), request.NewStatus))
            {
                throw new ArgumentException("Invalid application status provided.");
            }

            application.Status = request.NewStatus;
            _applicationRepository.Update(application);
            await _applicationRepository.SaveChangesAsync();

            return new JobApplicationResponse
            {
                Id = application.Id,
                UserId = application.UserId,
                JobPostingId = application.JobPostingId,
                Status = application.Status.ToString(),
                ApplicationDate = application.ApplicationDate
            };
        }

        public async Task<IEnumerable<JobApplicationResponse>> GetAllApplicationsForJobPostingAsync(Guid jobPostingId)
        {
            // This method is for administrators to see all applications for a given job posting
            var applications = await _applicationRepository.FindAsync(ja => ja.JobPostingId == jobPostingId);

            return applications.Select(ja => new JobApplicationResponse
            {
                Id = ja.Id,
                UserId = ja.UserId,
                JobPostingId = ja.JobPostingId,
                Status = ja.Status.ToString(),
                ApplicationDate = ja.ApplicationDate
            }).ToList();
        }

        public async Task<JobApplicationResponse> GetApplicationByIdAsync(Guid applicationId)
        {
            var application = await _applicationRepository.GetByIdAsync(applicationId);
            if (application == null)
            {
                return null;
            }
            return new JobApplicationResponse
            {
                Id = application.Id,
                UserId = application.UserId,
                JobPostingId = application.JobPostingId,
                Status = application.Status.ToString(),
                ApplicationDate = application.ApplicationDate
            };
        }
    }
}