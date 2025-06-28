namespace JobTracking.API.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using JobTracking.Application.Contracts;
    using JobTracking.Application.DTOs.Request;
    using JobTracking.Domain.Filters;
    using Microsoft.AspNetCore.Authorization;
    using System.Security.Claims;
    using JobTracking.Domain.Enums;

    [ApiController]
    [Route("api/[controller]")]
    public class JobPostingController : ControllerBase
    {
        private readonly IJobPostingService _jobPostingService;

        public JobPostingController(IJobPostingService jobPostingService)
        {
            _jobPostingService = jobPostingService;
        }

        [HttpPost]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> CreateJobPosting([FromBody] CreateJobPostingRequest request)
        {
            try
            {
                var jobPosting = await _jobPostingService.CreateJobPostingAsync(request);
                return CreatedAtAction(nameof(GetJobPostingById), new { id = jobPosting.Id }, jobPosting);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while creating the job posting.", details = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> UpdateJobPosting(Guid id, [FromBody] UpdateJobPostingRequest request)
        {
            try
            {
                var updatedJobPosting = await _jobPostingService.UpdateJobPostingAsync(id, request);
                return Ok(updatedJobPosting);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the job posting.", details = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> DeleteJobPosting(Guid id)
        {
            try
            {
                var result = await _jobPostingService.DeleteJobPostingAsync(id);
                if (!result)
                {
                    return NotFound(new { message = $"Job posting with ID {id} not found." });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the job posting.", details = ex.Message });
            }
        }

        [HttpGet]
        [Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> GetAllJobPostings([FromQuery] string? status, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            JobPostingStatus? statusFilter = null;
            if (!string.IsNullOrEmpty(status))
            {
                if (Enum.TryParse(typeof(JobPostingStatus), status, true, out var parsedStatus))
                {
                    statusFilter = (JobPostingStatus)parsedStatus;
                }
                else
                {
                    return BadRequest(new { message = $"Invalid status value: '{status}'. Valid values are 'Active' or 'Inactive'." });
                }
            }

            var filter = new JobPostingFilter
            {
                Status = statusFilter,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            try
            {
                var jobPostings = await _jobPostingService.GetAllJobPostingsAsync(filter);
                return Ok(jobPostings);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching job postings.", details = ex.Message });
            }
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> GetJobPostingById(Guid id)
        {
            try
            {
                var jobPosting = await _jobPostingService.GetJobPostingByIdAsync(id);
                if (jobPosting == null)
                {
                    return NotFound(new { message = $"Job posting with ID {id} not found." });
                }
                return Ok(jobPosting);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching the job posting.", details = ex.Message });
            }
        }
    }
}