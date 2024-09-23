using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApp_Sorteo.Helpers;
using WebApp_Sorteo.Models;
using WebApp_Sorteo.Models.Helpers;

namespace WebApp_Sorteo.Data.Inicializador
{
    public class DbInicializador : IDbInicializador
    {
        //* Inyectar servicios 
        #region Servicios a inyectar
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly string userName = "deyson19@mail.com";
        #endregion

        public DbInicializador(
            ApplicationDbContext dbContext,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager
            )
        {
            _dbContext = dbContext;
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public async Task MontarBaseDatos()
        {
            try
            {
                if (_dbContext.Database.GetPendingMigrations().Any())
                {
                    await _dbContext.Database.MigrateAsync();
                    Console.WriteLine("Se han aplicado las migraciones");
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            if (_dbContext.Roles.Any(x => x.Name == Roles.Role_Admin))
            {
                return;
            }
            else
            {
                await _roleManager.CreateAsync(new IdentityRole(Roles.Role_Admin));
                await _roleManager.CreateAsync(new IdentityRole(Roles.Role_Cliente));
                Console.WriteLine("Se han creado los roles");
            }

            var result = await _userManager.CreateAsync(new Usuario
            {
                UserName = userName,
                Email = userName,
                EmailConfirmed = true,
                Nombres = "Deyson",
                Apellidos = "Dev",
                Direccion = "Street 56 3s",
                Ciudad = "Bogota",
                Estado = "Bogota",
                Pais = "Colombia",
                Documento = "24257896"

            }, "Admin.dey_20");
            
            if (result.Succeeded)
            {
                var usuarioAdmin = await _dbContext.Users.Where(x => x.UserName == userName).FirstOrDefaultAsync()!;
                var roleResult = await _userManager.AddToRoleAsync(usuarioAdmin!, Roles.Role_Admin);

                if (!roleResult.Succeeded)
                {
                    foreach (var error in roleResult.Errors)
                    {
                        Console.WriteLine(error.Description);
                    }
                }
            }
            else
            {
                Console.WriteLine("No fue posible crear el usuario");
            }
        }
    }
    public interface IDbInicializador
    {
        Task MontarBaseDatos();
    }
}
