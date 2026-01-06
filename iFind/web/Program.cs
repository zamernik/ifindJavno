using web.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using web.Models;

var builder = WebApplication.CreateBuilder(args);

// Povezava na bazo
var connectionString = builder.Configuration.GetConnectionString("PovezavaDoiFindBazeAzure"); 
// LOKALNO: PovezavaDoiFindBaze // SERVER: PovezavaDoiFindBazeAzure

// Dodaj DbContext
builder.Services.AddDbContext<iFindContext>(options =>
    options.UseSqlServer(connectionString + ";TrustServerCertificate=True"));

// Dodaj Identity s svojimi ApplicationUser in vlogami
builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false; // ne zahteva potrditve maila
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<iFindContext>();

// Dodaj MVC in Razor Pages
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// Swagger (po želji)
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Middleware pipeline
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

// Swagger UI
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
});

// Map routes
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

    string[] roles = { "Uporabnik", "Administrator", "Organizator" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    string adminEmail = "admin@gmail.com";
    string adminPassword = "Admin123!";

    var adminUser = await userManager.FindByEmailAsync(adminEmail);
    if (adminUser == null)
    {
        adminUser = new ApplicationUser
{
        UserName = adminEmail,
        Email = adminEmail,
        EmailConfirmed = true,
        Ime = "Admin",
        Priimek = "Admin",
        Spol = "M" 
};

        var result = await userManager.CreateAsync(adminUser, adminPassword);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, "Administrator");
        }
    }
    else
    {
        if (!await userManager.IsInRoleAsync(adminUser, "Administrator"))
        {
            await userManager.AddToRoleAsync(adminUser, "Administrator");
        }
    }
}
// ================================================================

app.Run();
