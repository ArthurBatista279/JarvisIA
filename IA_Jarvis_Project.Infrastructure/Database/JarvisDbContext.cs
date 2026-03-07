using IA_Jarvis_Project.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace IA_Jarvis_Project.Infrastructure.Database
{
    public class JarvisDbContext : DbContext
    {
        public JarvisDbContext(DbContextOptions<JarvisDbContext> options) : base(options)
        {
        }

        public DbSet<MemoryRecord> Memories { get; set; }
    }
}