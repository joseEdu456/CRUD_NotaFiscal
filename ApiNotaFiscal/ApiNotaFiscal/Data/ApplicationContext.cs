using ApiNotaFiscal.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiNotaFiscal.Data;

public class ApplicationContext : DbContext
{
    public DbSet<NotaFiscal> NotaFiscal { get; set; }
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            "Data source=localhost\\SQLEXPRESS; Initial Catalog=ApiNotaFiscal; Integrated Security=true; TrustServerCertificate=True");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationContext).Assembly);
    }
}