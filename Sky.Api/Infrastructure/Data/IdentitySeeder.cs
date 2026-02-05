using Microsoft.AspNetCore.Identity;
using System.Data;

namespace Sky.Api.Infra.Data
{
    public static class IdentitySeeder
    {
        public static async Task SeedRolesAndUsersAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();

            await SeedRolesAsync(roleManager);
            await CreateUserAsync(userManager, configuration["SeedUsers:Admin:Email"]!, configuration["SeedUsers:Admin:Password"]!, "Admin");
            await CreateUserAsync(userManager, configuration["SeedUsers:Student:Email"]!, configuration["SeedUsers:Student:Password"]!, "Student");
        }

        private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            string[] roles = { "Admin", "Instructor", "Student" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    var result = await roleManager.CreateAsync(new IdentityRole(role));

                    if (!result.Succeeded)                   
                        throw new Exception($"Erro ao criar a role '{role}': {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }                
            }
        }

        private static async Task CreateUserAsync(UserManager<IdentityUser> userManager, string email, string password, string role)
        {
            if (await userManager.FindByEmailAsync(email) is not null)
                return;

            var user = new IdentityUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(user, password);
            if (!result.Succeeded)
                throw new Exception($"Erro ao criar o usuário '{email}': {string.Join(", ", result.Errors.Select(e => e.Description))}");

            await userManager.AddToRoleAsync(user, role);
        }
    }
}