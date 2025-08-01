using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using parcelPicUp.Data;
using parcelPicUp.Models;

var builder = WebApplication.CreateBuilder(args);

// Database connection string (appsettings.json থেকে নাও)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// DbContext যোগ করো
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Identity সেটআপ (Roles সহ)
builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false; // দরকারমত সেট করো
})
    .AddRoles<IdentityRole>()  // Roles ব্যবহার করার জন্য
    .AddEntityFrameworkStores<ApplicationDbContext>();

// MVC + Razor pages যোগ করো
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

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

// Route ম্যাপ করো
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Razor Pages ম্যাপ করো (Important for Identity UI)
app.MapRazorPages();

app.Run();
