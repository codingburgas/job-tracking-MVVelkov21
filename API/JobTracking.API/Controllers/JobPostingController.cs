namespace JobTracking.API.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using JobTracking.Application.Contracts;
    using JobTracking.Application.DTOs.Request;
    using JobTracking.Domain.Filters;
    using Microsoft.AspNetCore.Authorization;
    using System.Security.Claims; // For accessing user roles

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
        [Authorize(Policy = "AdminPolicy")] // Only Admins can create job postings
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
        [Authorize(Policy = "AdminPolicy")] // Only Admins can update job postings
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
        [Authorize(Policy = "AdminPolicy")] // Only Admins can delete job postings
        public async Task<IActionResult> DeleteJobPosting(Guid id)
        {
            try
            {
                var result = await _jobPostingService.DeleteJobPostingAsync(id);
                if (!result)
                {
                    return NotFound(new { message = $"Job posting with ID {id} not found." });
                }
                return NoContent(); // 204 No Content for successful deletion
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the job posting.", details = ex.Message });
            }
        }

        [HttpGet]
        [Authorize(Policy = "UserPolicy")] // Both users and admins can view job postings
        public async Task<IActionResult> GetAllJobPostings([FromQuery] JobPostingFilter filter)
        {
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
        [Authorize(Policy = "UserPolicy")] // Both users and admins can view a specific job posting
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