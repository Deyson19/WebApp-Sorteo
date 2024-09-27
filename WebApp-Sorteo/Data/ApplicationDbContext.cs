using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System.Reflection;
using WebApp_Sorteo.Data.Inicializador;
using WebApp_Sorteo.Models;

namespace WebApp_Sorteo.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {   
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
          
        }
        public DbSet<Sorteo> Sorteos { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Premio> Premios { get; set; }
        public DbSet<Pago> Pagos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
