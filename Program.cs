using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Task_Manager.Models;

var builder = WebApplication.CreateBuilder(args);

// Register DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Register Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// Configure cookie (optional)
builder.Services.ConfigureApplicationCookie(options =>
{
    options.AccessDeniedPath = "/Shared/AccessDenied";
});

builder.Services.AddControllersWithViews();

var app = builder.Build();

// ? Seed Admin User & Role
using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    string adminEmail = "admin@gmail.com";
    string adminPassword = "Admin@123";
    string roleName = "Admin";

    // Ensure role exists
    if (!await roleManager.RoleExistsAsync(roleName))
    {
        await roleManager.CreateAsync(new IdentityRole(roleName));
    }

    // Ensure user exists
    var adminUser = await userManager.FindByEmailAsync(adminEmail);
    if (adminUser == null)
    {
        adminUser = new ApplicationUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            EmailConfirmed = true
        };
        var result = await userManager.CreateAsync(adminUser, adminPassword);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, roleName);
        }
    }
}

app.UseAuthentication();
app.UseAuthorization();

app.MapDefaultControllerRoute();
app.Run();
