using Microsoft.EntityFrameworkCore;
using Company.Data.Data;
namespace Company.Intranet
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<CompanyContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("CompanyContext") ?? throw new InvalidOperationException("Connection string 'CompanyContext' not found.")));

            // Add services to the container.
            builder.Services.AddControllersWithViews();

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
            // Auto-migracja bazy danych przy starcie (tylko DEV!)
            if (app.Environment.IsDevelopment())
            {
                using (var scope = app.Services.CreateScope())
                {
                    try
                    {
                        var dbContext = scope.ServiceProvider.GetRequiredService<CompanyContext>();
                        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

                        // Migracja bazy danych
                        dbContext.Database.Migrate();
                        logger.LogInformation("✅ Baza danych zmigrowana pomyślnie!");
                    }
                    catch (Exception ex)
                    {
                        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                        logger.LogError(ex, "⚠️ Błąd podczas migracji bazy danych");
                        Console.WriteLine($"⚠️ Błąd połączenia z bazą: {ex.Message}");
                        Console.WriteLine("ℹ️ Uruchom: docker-compose -f docker-compose.yml up -d");
                    }
                }
            }
            app.Run();
        }
    }
}
