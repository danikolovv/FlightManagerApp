using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using FlightManagerApp.Data;
using FlightManagerApp.Areas.Identity.Data;
using AutoMapper;

namespace FlightManagerApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var connectionString = builder.Configuration.GetConnectionString("FlightManagerContextConnection") ?? throw new InvalidOperationException("Connection string 'FlightManagerContextConnection' not found.");

            builder.Services.AddDbContext<FlightManagerContext>(options => options.UseSqlServer(connectionString));

            builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<FlightManagerContext>();

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();

            builder.Services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireUppercase = false;
            });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
                options.AddPolicy("Employee", policy => policy.RequireRole("Employee"));
                options.AddPolicy("Customer", policy => policy.RequireRole("Customer"));
            });

            builder.Services.AddTransient<IEmailSender, EmailSender>();

            builder.Services.AddAutoMapper(typeof(Program));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();


            using (var scope = app.Services.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                // Get role IDs
                var adminRoleId = (await roleManager.FindByNameAsync("Admin"))?.Id;
                var employeeRoleId = (await roleManager.FindByNameAsync("Employee"))?.Id;

                // Ensure roles exist
                if (adminRoleId == null || employeeRoleId == null)
                {
                    throw new ApplicationException("Roles not found.");
                }

                // Assign roles to users
                var adminUserId = "94b0995c-0b00-4e9a-aec5-854984903bb5"; // Assuming this is the admin user's ID

                var adminUser = await userManager.FindByIdAsync(adminUserId);
                if (adminUser != null)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
                else
                {
                    throw new ApplicationException("Admin user not found.");
                }

                // List of employee user IDs
                var employeeUserIds = new List<string>
                {
                    "fe62c86c-5646-47d9-9e29-89766214e87c"
                };

                foreach (var employeeUserId in employeeUserIds)
                {
                    var employeeUser = await userManager.FindByIdAsync(employeeUserId);
                    if (employeeUser != null)
                    {
                        await userManager.AddToRoleAsync(employeeUser, "Employee");
                    }
                    else
                    {
                        throw new ApplicationException("Employee user not found.");
                    }
                }
            }


            app.Run();
        }
        /*public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(EmployeeProfile));
        }*/
    }
}
