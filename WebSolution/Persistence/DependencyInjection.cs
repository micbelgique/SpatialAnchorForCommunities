using Microsoft.AspNetCore.Identity;
using Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Domain.Entities;

namespace Persistence
{
    public static class DependencyInjection
    {
        public static void AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                
                options.UseSqlServer(
                    configuration.GetConnectionString("AzureConnection"),
                    x => x.UseNetTopologySuite());
                
            });


            services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());
        }

        public static void Configure(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            IdentityDataInitializer.SeedData(userManager, roleManager);
        }
    }
}
