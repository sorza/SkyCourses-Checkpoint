using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Sky.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
           
            if (builder.Environment.IsDevelopment())
            {               
                builder.Services.AddDbContext<Infra.Data.AppDbContext>(options =>
                    options.UseSqlite(builder.Configuration.GetConnectionString("SqliteConnection")));
            }
            else
            {               
                builder.Services.AddDbContext<Infra.Data.AppDbContext>(options =>
                    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            }

            builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 8;
            })
             .AddEntityFrameworkStores<Infra.Data.AppDbContext>()
             .AddDefaultTokenProviders();

            builder.Services.AddAuthorization();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapGet("/health", () =>
            {
                return "Healthy";
            });

            app.Run();
        }
    }
}
