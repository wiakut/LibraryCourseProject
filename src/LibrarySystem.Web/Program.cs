using LibrarySystem.Application;
using LibrarySystem.Infrastructure;
using LibrarySystem.Infrastructure.Data;
using LibrarySystem.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Globalization;
var builder = WebApplication.CreateBuilder(args);
var cultureInfo = new CultureInfo("en-US");
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
builder.Services.AddControllersWithViews();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    options.SlidingExpiration = true;
});
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("Reader", policy => policy.RequireRole("Reader"));
});
var app = builder.Build();
app.Use((context, next) =>
{
    var cultureInfo = new CultureInfo("en-US");
    CultureInfo.CurrentCulture = cultureInfo;
    CultureInfo.CurrentUICulture = cultureInfo;
    return next();
});
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    var maxRetries = 30;
    var retryDelay = TimeSpan.FromSeconds(2);
    var retryCount = 0;
    var connected = false;
    while (retryCount < maxRetries && !connected)
    {
        try
        {
            logger.LogInformation("Attempting to connect to database (attempt {Attempt}/{MaxRetries})...", retryCount + 1, maxRetries);
            await context.Database.CanConnectAsync();
            connected = true;
            logger.LogInformation("Database connection successful!");
        }
        catch (Exception ex)
        {
            retryCount++;
            if (retryCount >= maxRetries)
            {
                logger.LogError(ex, "Failed to connect to database after {MaxRetries} attempts. Error: {Message}", maxRetries, ex.Message);
                throw;
            }
            logger.LogWarning("Database connection failed. Retrying in {Delay} seconds... (attempt {Attempt}/{MaxRetries})", retryDelay.TotalSeconds, retryCount, maxRetries);
            await Task.Delay(retryDelay);
        }
    }
    try
    {
        var pendingMigrations = await context.Database.GetPendingMigrationsAsync();
        if (pendingMigrations.Any())
        {
            logger.LogInformation("Applying {Count} pending migration(s)...", pendingMigrations.Count());
            await context.Database.MigrateAsync();
            logger.LogInformation("Migrations applied successfully.");
        }
        else
        {
            logger.LogInformation("Database is up to date. No pending migrations.");
        }
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred while applying migrations. Error: {Message}", ex.Message);
    }
    try
    {
        await IdentitySeedData.SeedRolesAsync(scope.ServiceProvider);
        await IdentitySeedData.SeedAdminUserAsync(scope.ServiceProvider);
        await IdentitySeedData.SeedReaderCategoriesAsync(scope.ServiceProvider);
        await IdentitySeedData.SeedTestReaderUsersAsync(scope.ServiceProvider);
        await IdentitySeedData.SeedDemoDataAsync(scope.ServiceProvider);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred while seeding data. Error: {Message}", ex.Message);
    }
}
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");
app.Run();