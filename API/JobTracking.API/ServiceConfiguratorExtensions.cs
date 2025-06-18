namespace JobTracking.API
{
    using Microsoft.Extensions.DependencyInjection;
    using JobTracking.Application.Contracts;
    using JobTracking.Application.Implementation;
    using JobTracking.DataAccess.Repositories;
    using JobTracking.DataAccess.Data;
    using Microsoft.Extensions.Configuration;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.IdentityModel.Tokens;
    using System.Text;
    using JobTracking.Domain.Constants;
    using JobTracking.Domain.Entities; // Needed for generic repository registration

    public static class ServiceConfiguratorExtensions
    {
        public static void AddJobTrackingServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Configure DbContext
            services.AddDbContext<AppDbContext>(options =>
                    options.UseSqlite("Data Source=JobTracking.db")
            );

            // Register Repositories
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            // Register Application Services
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IJobPostingService, JobPostingService>();
            services.AddScoped<IJobApplicationService, JobApplicationService>();

            // Configure JWT Authentication
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Jwt.Issuer,
                    ValidAudience = Jwt.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Jwt.Key))
                };
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminPolicy", policy => policy.RequireRole(JobTracking.Domain.Enums.UserRole.Admin.ToString()));
                options.AddPolicy("UserPolicy", policy => policy.RequireRole(JobTracking.Domain.Enums.UserRole.User.ToString(), JobTracking.Domain.Enums.UserRole.Admin.ToString()));
            });

            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
        }
    }
}