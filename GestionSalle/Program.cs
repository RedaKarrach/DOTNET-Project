using GestionSalle.Models;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
// Configure Entity Framework Core
builder.Services.AddDbContext<SalleDbContext>(options =>
 options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Apply any pending migrations at startup
using (var scope = app.Services.CreateScope())
{
 var services = scope.ServiceProvider;
 var logger = services.GetRequiredService<ILogger<Program>>();
 try
 {
 var context = services.GetRequiredService<SalleDbContext>();
 context.Database.Migrate();

 // Diagnostic: verify connectivity and list tables
 if (!context.Database.CanConnect())
 {
 logger.LogError("Cannot connect to the database using the configured connection string.");
 }
 else
 {
 logger.LogInformation("Successfully connected to the database.");
 try
 {
 DbConnection conn = context.Database.GetDbConnection();
 conn.Open();
 using var cmd = conn.CreateCommand();
 cmd.CommandText = "SELECT TABLE_SCHEMA + '.' + TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE='BASE TABLE'";
 using var reader = cmd.ExecuteReader();
 while (reader.Read())
 {
 logger.LogInformation("Found table: {table}", reader.GetString(0));
 }
 conn.Close();
 }
 catch (Exception ex)
 {
 logger.LogError(ex, "Error while listing tables from the database.");
 }
 }
 }
 catch (Exception ex)
 {
 logger?.LogError(ex, "An error occurred while migrating the database.");
 // If you prefer the app to stop on migration failure, rethrow here:
 // throw;
 }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
 app.UseExceptionHandler("/Home/Error");
 // The default HSTS value is30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
 app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
 name: "default",
 pattern: "{controller=Home}/{action=Index}/{id?}")
 .WithStaticAssets();

app.Run();