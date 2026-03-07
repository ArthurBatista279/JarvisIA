using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace IA_Jarvis_Project.Infrastructure.Database
{
    public class JarvisDbContextFactory : IDesignTimeDbContextFactory<JarvisDbContext>
    {
        public JarvisDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<JarvisDbContext>();
            // O caminho do banco de dados deve ser absoluto ou relativo à pasta de execução
            optionsBuilder.UseSqlite("Data Source=jarvis_memory.db");

            return new JarvisDbContext(optionsBuilder.Options);
        }
    }
}