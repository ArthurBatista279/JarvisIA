namespace IA_Jarvis_Project.Web;
using IA_Jarvis_Project.Core.Interfaces;
using IA_Jarvis_Project.Infrastructure.AI;
using IA_Jarvis_Project.Infrastructure.Database;
using IA_Jarvis_Project.Infrastructure.Repository;
using IA_Jarvis_Project.Services;
using Microsoft.EntityFrameworkCore;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // --- UI ---
        builder.Services.AddRazorPages();

        // --- Banco de Dados (SQLite + EF Core) ---
        builder.Services.AddDbContext<JarvisDbContext>(options =>
            options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

        // --- Repositórios ---
        builder.Services.AddScoped<IMemoryRepository, MemoryRepository>();

        // --- Motor de IA (Gemini) ---
        builder.Services.AddHttpClient<IAIService, GeminiService>();

        // --- Serviços do Jarvis ---
        builder.Services.AddScoped<ChatService>();
        builder.Services.AddScoped<MemoryService>();
        builder.Services.AddScoped<CommandService>();

        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<JarvisDbContext>();
            db.Database.EnsureCreated();
        }

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthorization();
        app.MapStaticAssets();
        app.MapRazorPages().WithStaticAssets();

        app.Run();
    }
}
