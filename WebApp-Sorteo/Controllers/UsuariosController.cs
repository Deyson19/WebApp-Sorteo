using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using WebApp_Sorteo.Data;
using WebApp_Sorteo.Helpers;

namespace WebApp_Sorteo.Controllers
{
    public class UsuariosController(ApplicationDbContext context) : Controller
    {

        private readonly ApplicationDbContext _dbContext = context;
        public async Task<IActionResult> Index()
        {
            var listadoUsuarios = await _dbContext.Usuarios.ToListAsync();
            var listadoUsuariosRoles = await _dbContext.UserRoles.ToListAsync();
            var listadoRoles = await _dbContext.Roles.ToListAsync();
            foreach (var usuario in listadoUsuarios)
            {
                var roleId = listadoUsuariosRoles.Find(x => x.UserId == usuario.Id)!.RoleId;
                usuario.Role = listadoRoles.Find(x => x.Id == roleId)!.Name;
            }
            return View(listadoUsuarios);
        }
        [HttpPost]
        public async Task<IActionResult> LockUnlock([FromBody] string id)
        {
            if (id.IsNullOrEmpty())
            {
                TempData[DS.Error] = "Por favor comprueba los datos";
                return Json(new
                {
                    message = "Por favor comprueba los datos"
                });
            }
            var usuario = await _dbContext.Usuarios.FirstOrDefaultAsync(x => x.Id == id);
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (currentUserId == usuario!.Id)
            {
                return Json(new
                {
                    success = false,
                    message = "Error al realizar la solicitud."
                });
            }
            if (usuario is not null)
            {
                if (usuario.LockoutEnd != null && usuario.LockoutEnd > DateTime.Now)
                {
                    usuario.LockoutEnd = DateTime.Now;
                }
                else
                {
                    usuario.LockoutEnd = DateTime.Now.AddMonths(2);
                }
                _dbContext.Usuarios.Update(usuario);
                await _dbContext.SaveChangesAsync();
                return Json(new { success = true, message = "Se ha procesado la solicitud."});
            }
            TempData[DS.Error] = "No se ha encontrado el usuario";
            return Json(new
            {
                success = false,
                message = "Error al realizar la solicitud."
            });
        }
    }
}
