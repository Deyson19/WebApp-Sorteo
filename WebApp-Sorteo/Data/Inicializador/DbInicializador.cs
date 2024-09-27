using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApp_Sorteo.Models;
using WebApp_Sorteo.Models.Helpers;

namespace WebApp_Sorteo.Data.Inicializador
{
    public class DbInicializador(
        ApplicationDbContext dbContext,
        UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager
            ) : IDbInicializador
    {
        #region Servicios a inyectar
        private readonly ApplicationDbContext _dbContext = dbContext;
        private readonly UserManager<IdentityUser> _userManager = userManager;
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;
        private readonly string userName = "deyson19@mail.com";

        #endregion
        public void MontarBaseDatos()
        {
            if (_dbContext.Database.GetPendingMigrations().Any())
            {
                _dbContext.Database.Migrate();
            }
            if (_dbContext.Roles.Any(x => x.Name == Roles.Role_Admin)) return;

            _roleManager.CreateAsync(new IdentityRole(Roles.Role_Admin)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(Roles.Role_Cliente)).GetAwaiter().GetResult();

            _userManager.CreateAsync(new Usuario
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

            }, "Admin.dey_20").GetAwaiter().GetResult();

            var usuarioAdmin = _dbContext.Usuarios.Local.FirstOrDefault(x => x.UserName == userName);
            _userManager.AddToRoleAsync(usuarioAdmin!, Roles.Role_Admin).GetAwaiter().GetResult();           
        }
    }
    public interface IDbInicializador
    {
        void MontarBaseDatos();
    }
}
