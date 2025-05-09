namespace JobTracking.API
{
    public static class ServiceConfiguratorExtensions
    {
        public static void AddContext(this WebApplicationBuilder builder)
        {
            //builder.Services.AddDbContext<HrManagementContext>(options =>
            //{
            //    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            //});
        }
 
        public static void AddIdentity(this WebApplicationBuilder builder)
        {
            //builder.Services.AddIdentityCore<IdentityUser>()
            //    .AddRoles<IdentityRole>()
            //    .AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>("HrManagement")
            //    .AddEntityFrameworkStores<HrManagementContext>()
            //    .AddDefaultTokenProviders();
 
            //builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            //{
            //    options.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        ValidateIssuer = true,
            //        ValidateAudience = true,
            //        ValidateLifetime = true,
            //        ValidateIssuerSigningKey = true,
            //        ValidIssuer = builder.Configuration[Jwt.Issuer],
            //        ValidAudience = builder.Configuration[Jwt.Audience],
            //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration[Jwt.Key]))
            //    };
            //});
        }
 
        public static void AddServices(this WebApplicationBuilder builder)
        {
            // ...
        }
 
        public static void AddCors(this WebApplicationBuilder builder)
        {
            //builder.Services.AddCors(options =>
            //{
            //    options.AddDefaultPolicy(
            //        config =>
            //        {
            //            config.WithOrigins("http://localhost:4200", "https://localhost:7184")
            //                .AllowAnyHeader()
            //                .WithMethods(HttpMethod.Get.Method, HttpMethod.Post.Method, HttpMethod.Put.Method,
            //                    HttpMethod.Delete.Method);
            //        });
            //});
        }
    }
}