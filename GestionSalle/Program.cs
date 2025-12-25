using GestionSalle.Models;
using Microsoft.EntityFrameworkCore;
using GestionSalle.Repositories;
using GestionSalle.Services;
using GestionSalle.Filters;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<GlobalExceptionFilter>();
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
builder.Services.AddScoped<IEntraineurService, EntraineurService>();
builder.Services.AddScoped<IUtilisateurService, UtilisateurService>();
builder.Services.AddScoped<IPasswordService, PasswordService>();

// Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.Cookie.Name = "GestionSalleAuth";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
    });

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(2);
});

var app = builder.Build();

// Seed default admin if missing and default plans
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();
    try
    {
        var context = services.GetRequiredService<SalleDbContext>();
        context.Database.Migrate();

        // Seed admin user
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

        // Seed default plans if missing
        var planRepo = services.GetRequiredService<IRepository<GestionSalle.Models.PlanAbonnement>>();
        var existingPlans = await planRepo.GetAllAsync();
        if (!existingPlans.Any())
        {
            var defaultPlans = new List<GestionSalle.Models.PlanAbonnement>
            {
                new GestionSalle.Models.PlanAbonnement
                {
                    Nom = "Plan Basique",
                    Description = "Accès à la salle de sport avec équipements de base",
                    Prix = 200,
                    DureeEnMois = 1
                },
                new GestionSalle.Models.PlanAbonnement
                {
                    Nom = "Plan Standard",
                    Description = "Accès à la salle + cours collectifs",
                    Prix = 500,
                    DureeEnMois = 3
                },
                new GestionSalle.Models.PlanAbonnement
                {
                    Nom = "Plan Premium",
                    Description = "Accès complet + entraîneur personnel + cours collectifs",
                    Prix = 1500,
                    DureeEnMois = 6
                },
                new GestionSalle.Models.PlanAbonnement
                {
                    Nom = "Plan Annuel",
                    Description = "Accès complet pour toute l'année avec tous les services",
                    Prix = 2500,
                    DureeEnMois = 12
                }
            };

            foreach (var plan in defaultPlans)
            {
                await planRepo.AddAsync(plan);
            }
            await planRepo.SaveChangesAsync();
            logger.LogInformation($"Seeded {defaultPlans.Count} default plans");
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