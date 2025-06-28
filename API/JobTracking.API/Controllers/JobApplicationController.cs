namespace JobTracking.API.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using JobTracking.Application.Contracts;
    using JobTracking.Application.DTOs.Request;
    using Microsoft.AspNetCore.Authorization;
    using System.Security.Claims;
    using JobTracking.Domain.Enums;

    [ApiController]
    [Route("api/[controller]")]
    public class JobApplicationController : ControllerBase
    {
        private readonly IJobApplicationService _jobApplicationService;
        private readonly IUserService _userService;

        public JobApplicationController(IJobApplicationService jobApplicationService, IUserService userService)
        {
            _jobApplicationService = jobApplicationService;
            _userService = userService;
        }

        [HttpPost("apply")]
        [Authorize(Policy = "UserPolicy")]
        
        public async Task<IActionResult> ApplyForJob([FromBody] ApplyForJobRequest request)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new { message = "Invalid user ID." });
            }
            
            if (User.IsInRole(UserRole.Admin.ToString()))
            {
                return Forbid("Administrators cannot submit job applications.");
            }

            try
            {
                var application = await _jobApplicationService.ApplyForJobAsync(userId, request);
                return CreatedAtAction(nameof(GetJobApplicationById), new { applicationId = application.Id }, application);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, new { message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred during application submission.", details = ex.Message });
            }
        }

        [HttpGet("my-applications")]
        [Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> GetMyApplications()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new { message = "Invalid user ID." });
            }

            try
            {
                var applications = await _jobApplicationService.GetUserApplicationsAsync(userId);
                return Ok(applications);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching your applications.", details = ex.Message });
            }
        }

        [HttpPatch("{applicationId}/status")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> UpdateApplicationStatus(Guid applicationId, [FromBody] UpdateApplicationStatusRequest request)
        {
            try
            {
                var updatedApplication = await _jobApplicationService.UpdateApplicationStatusAsync(applicationId, request);
                return Ok(updatedApplication);
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
                return StatusCode(500, new { message = "An error occurred while updating application status.", details = ex.Message });
            }
        }

        [HttpGet("job/{jobPostingId}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> GetAllApplicationsForJobPosting(Guid jobPostingId)
        {
            try
            {
                var applications = await _jobApplicationService.GetAllApplicationsForJobPostingAsync(jobPostingId);
                return Ok(applications);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching applications for the job posting.", details = ex.Message });
            }
        }

        [HttpGet("{applicationId}")]
        [Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> GetJobApplicationById(Guid applicationId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userIdClaim, out var currentUserId))
            {
                return Unauthorized(new { message = "Invalid user ID." });
            }

            try
            {
                var application = await _jobApplicationService.GetApplicationByIdAsync(applicationId);
                if (application == null)
                {
                    return NotFound(new { message = $"Application with ID {applicationId} not found." });
                }
                
                if (!User.IsInRole(UserRole.Admin.ToString()) && application.UserId != currentUserId)
                {
                    return Forbid("You do not have access to this application.");
                }

                return Ok(application);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching the application.", details = ex.Message });
            }
        }
    }
}