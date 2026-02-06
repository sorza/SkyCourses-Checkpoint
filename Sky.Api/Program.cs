using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Sky.Api.Application.Interfaces;
using Sky.Api.Endpoints;
using Sky.Api.Infrastructure.Data;
using Sky.Api.Infrastructure.Implementation;
using System.Text;

namespace Sky.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Registrando o DbContext
            if (builder.Environment.IsDevelopment())
            {               
                builder.Services.AddDbContext<AppDbContext>(options =>
                    options.UseSqlite(builder.Configuration.GetConnectionString("SqliteConnection")));
            }
            else
            {               
                builder.Services.AddDbContext<AppDbContext>(options =>
                    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            }
            #endregion

            #region Identity Configurations

            builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 8;

                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15); 
                options.Lockout.MaxFailedAccessAttempts = 5; 
                options.Lockout.AllowedForNewUsers = true;

                options.User.RequireUniqueEmail = true;

                if (builder.Environment.IsProduction())
                {
                    options.SignIn.RequireConfirmedEmail = true;
                    options.SignIn.RequireConfirmedPhoneNumber = true;
                    options.SignIn.RequireConfirmedAccount = true;
                }
                else
                {
                    options.SignIn.RequireConfirmedEmail = false;
                    options.SignIn.RequireConfirmedPhoneNumber = false;
                    options.SignIn.RequireConfirmedAccount = false;
                }
            })
             .AddEntityFrameworkStores<AppDbContext>()
             .AddDefaultTokenProviders();
            #endregion

            #region JWT Authentication
                       
            var jwtIssuer = builder.Configuration["Jwt:Issuer"]
                ?? throw new InvalidOperationException("Jwt:Issuer não configurado");
            var jwtAudience = builder.Configuration["Jwt:Audience"]
                ?? throw new InvalidOperationException("Jwt:Audience não configurado");
            var jwtSecretKey = builder.Configuration["Jwt:SecretKey"]
                ?? throw new InvalidOperationException("Jwt:SecretKey não configurado");

            var key = Encoding.UTF8.GetBytes(jwtSecretKey);

            builder.Services.AddAuthentication(options =>
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
                    ValidIssuer = jwtIssuer,
                    ValidAudience = jwtAudience,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ClockSkew = TimeSpan.Zero 
                };
            });

            #endregion

            builder.Services.AddAuthorization();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            #region Registrando serviços 
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<ITokenService, TokenService>();

            #endregion

            var app = builder.Build();

            #region Database Seed
           
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    await IdentitySeeder.SeedRolesAndUsersAsync(services);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "Erro ao executar o seed de dados.");
                }
            }

            #endregion

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
                       

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            #region Endpoints

            app.MapGet("/health", () =>
            {
                return "Healthy";
            });

            app.MapEndpoints();

            #endregion          

            app.Run();
        }
    }
}
