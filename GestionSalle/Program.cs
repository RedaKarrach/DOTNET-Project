using GestionSalle.Models;
using Microsoft.EntityFrameworkCore;
using GestionSalle.Repositories;
using GestionSalle.Services;
using GestionSalle.Filters;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<GlobalExceptionFilter>();
});

// Enforce authentication by default (mask pages until login)
builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

// Configure Entity Framework Core
builder.Services.AddDbContext<SalleDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repositories and services
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IMembreRepository, MembreRepository>();
builder.Services.AddScoped<IPaiementRepository, PaiementRepository>();
builder.Services.AddScoped<IMembreService, MembreService>();
builder.Services.AddScoped<IPaiementService, PaiementService>();
builder.Services.AddScoped<IUtilisateurService, UtilisateurService>();
builder.Services.AddScoped<IPasswordService, PasswordService>();

// Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.Cookie.Name = "GestionSalleAuth";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
    });

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(2);
});

var app = builder.Build();

// Seed default admin if missing
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();
    try
    {
        var context = services.GetRequiredService<SalleDbContext>();
        context.Database.Migrate();

        var pwdSvc = services.GetRequiredService<IPasswordService>();
        var repo = services.GetRequiredService<IRepository<GestionSalle.Models.Utilisateur>>();
        var users = await repo.GetAllAsync();
        if (!users.Any(u => u.NomUtilisateur == "admin"))
        {
            var admin = new GestionSalle.Models.Utilisateur { NomUtilisateur = "admin", MotDePasse = pwdSvc.HashPassword("Admin@123"), Role = "Admin" };
            await repo.AddAsync(admin);
            await repo.SaveChangesAsync();
            logger.LogInformation("Seeded admin user");
        }
    }
    catch (Exception ex)
    {
        var logger2 = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger2.LogError(ex, "Error during migration or seeding");
    }
}

// Configure the HTTP request pipeline.
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
app.UseSession();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();