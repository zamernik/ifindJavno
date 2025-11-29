using web.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using web.Models;

var builder = WebApplication.CreateBuilder(args);

//alternativa temu kar je on naredil v startup.cs, ki tukaj ne obstaja več
//spremenil sem ime na stežniško verzijo
var connectionString = builder.Configuration.GetConnectionString("PovezavaDoiFindBaze"); //LOKALNO: PovezavaDoiFindBaze //SERVER: PovezavaDoiFindBazeAzure

// Dodaj DbContext
builder.Services.AddDbContext<iFindContext>(options =>
    options.UseSqlServer(connectionString + ";TrustServerCertificate=True"));

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages(); // dodano tukaj, pred builder.Build()

// Dodaj Identity s svojim ApplicationUser in vloge
builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false; // ne zahteva potrditve maila
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<iFindContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages(); // dodano tukaj, po UseAuthorization

app.Run();
