using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Simple_API_Assessment.Data.Repository;
using SimpleA.Data;
using System;
using System.Text.Json.Serialization;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllers();

        // Add DbContext configuration
        var connectionString = builder.Configuration.GetConnectionString("SimpleApiConnection");
        builder.Services.AddDbContext<DataContext>(options =>
            options.UseNpgsql(connectionString));

        // Register repositories
        builder.Services.AddScoped<IApplicantRepository, ApplicantRepo>();

        // Configure JSON serialization options
        builder.Services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
        });

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddControllersWithViews(); // This line should be within the services configuration

        var app = builder.Build();

        // Ensure database is created and apply migrations
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            try
            {
                var dbContext = services.GetRequiredService<DataContext>();
                // Apply pending migrations to ensure database schema is up-to-date
                dbContext.Database.Migrate();

                // Call the existing SeedData method defined in DataContext
                dbContext.SeedData();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An error occurred while seeding the database: {ex.Message}");
            }
        }

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
        });

        app.Run();
    }

}
