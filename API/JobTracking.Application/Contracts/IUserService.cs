namespace JobTracking.Application.Contracts
{
    using JobTracking.Application.DTOs.Request;
    using JobTracking.Application.DTOs.Response;

    public interface IUserService
    {
        Task<UserResponse> RegisterUserAsync(UserRegistrationRequest request);
        Task<string> AuthenticateUserAsync(UserLoginRequest request);
        Task<UserResponse> GetUserProfileAsync(Guid userId);
        Task<bool> UserExistsAsync(Guid userId);
    }
}