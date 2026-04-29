using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BaeLilyDesigns.Data;
using BaeLilyDesigns.Models;
using BaeLilyDesigns.Services;

var builder = WebApplication.CreateBuilder(args);

// Database: SQL Server if connection string looks like it, otherwise SQLite
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "";
var useSqlite = string.IsNullOrWhiteSpace(connectionString)
    || connectionString.Contains("Data Source=", StringComparison.OrdinalIgnoreCase);

if (useSqlite)
{
    var sqliteConn = builder.Configuration.GetConnectionString("SQLiteConnection")
                    ?? "Data Source=baelily.db";
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlite(sqliteConn));
}
else
{
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(connectionString));
}

// Identity — use fully qualified ApplicationUser so the compiler always resolves it
builder.Services
    .AddDefaultIdentity<BaeLilyDesigns.Models.ApplicationUser>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false;
        options.Password.RequireDigit = true;
        options.Password.RequiredLength = 6;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath    = "/Account/Login";
    options.LogoutPath   = "/Account/Logout";
    options.AccessDeniedPath = "/Account/AccessDenied";
});

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(24);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddScoped<EmailService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

// Seed roles, admin account and demo products on startup
using (var scope = app.Services.CreateScope())
{
    await BaeLilyDesigns.Data.DbInitializer.Seed(scope.ServiceProvider);
}

app.Run();
