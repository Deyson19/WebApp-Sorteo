using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using WebApp_Sorteo.Models;

namespace WebApp_Sorteo.Data
{
     public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext(options)
     {
          public DbSet<Sorteo>? Sorteos { get; set; }
          public DbSet<Ticket>? Tickets { get; set; }
          public DbSet<Premio>? Premios { get; set; }
          public DbSet<Pago>? Pagos { get; set; }
          public DbSet<Usuario>? Usuarios { get; set; }
          protected override void OnModelCreating(ModelBuilder builder)
          {
               base.OnModelCreating(builder);
               builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
          }
     }
}
