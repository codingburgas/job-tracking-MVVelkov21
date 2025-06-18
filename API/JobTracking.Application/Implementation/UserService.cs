namespace JobTracking.Application.Implementation
{
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using Microsoft.IdentityModel.Tokens;
    using BCrypt.Net;
    using JobTracking.Application.Contracts;
    using JobTracking.Application.DTOs.Request;
    using JobTracking.Application.DTOs.Response;
    using JobTracking.DataAccess.Repositories;
    using JobTracking.Domain.Entities;
    using JobTracking.Domain.Enums;
    using JobTracking.Domain.Constants;

    public class UserService : IUserService
    {
        private readonly IRepository<User> _userRepository;

        public UserService(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserResponse> RegisterUserAsync(UserRegistrationRequest request)
        {
            // Basic validation
            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            {
                throw new ArgumentException("Username and password are required.");
            }

            // Check if username already exists
            var existingUser = (await _userRepository.FindAsync(u => u.Username == request.Username)).FirstOrDefault();
            if (existingUser != null)
            {
                throw new InvalidOperationException("Username already exists.");
            }

            var user = new User
            {
                Id = Guid.NewGuid(),
                FirstName = request.FirstName,
                LastName = request.LastName,
                Username = request.Username,
                PasswordHash = BCrypt.HashPassword(request.Password), // Hash the password
                Role = UserRole.User // Default role for registration
            };

            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            return new UserResponse { Id = user.Id, FirstName = user.FirstName, LastName = user.LastName, Username = user.Username, Role = user.Role.ToString() };
        }

        public async Task<string> AuthenticateUserAsync(UserLoginRequest request)
        {
            var user = (await _userRepository.FindAsync(u => u.Username == request.Username)).FirstOrDefault();

            if (user == null || !BCrypt.Verify(request.Password, user.PasswordHash))
            {
                return null;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Jwt.Key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.Role.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(Jwt.TokenExpirationMinutes),
                Issuer = Jwt.Issuer,
                Audience = Jwt.Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<UserResponse> GetUserProfileAsync(Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return null;
            }
            return new UserResponse { Id = user.Id, FirstName = user.FirstName, LastName = user.LastName, Username = user.Username, Role = user.Role.ToString() };
        }

        public async Task<bool> UserExistsAsync(Guid userId)
        {
            return (await _userRepository.GetByIdAsync(userId)) != null;
        }
    }
}