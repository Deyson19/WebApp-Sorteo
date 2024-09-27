using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp_Sorteo.Data;
using WebApp_Sorteo.Models;

namespace WebApp_Sorteo.Controllers
{
    public class SorteosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SorteosController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Sorteos.Include(s => s.Premio);
            return View(await applicationDbContext.ToListAsync());
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sorteo = await _context.Sorteos
                .Include(s => s.Premio)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sorteo == null)
            {
                return NotFound();
            }

            return View(sorteo);
        }

        public IActionResult Create()
        {
            ViewData["PremioId"] = new SelectList(_context.Premios, "Id", "Descripcion");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Sorteo sorteo)
        {
            if (ModelState.IsValid)
            {
                await _context.AddAsync(sorteo);
                await _context.SaveChangesAsync();
                for (int i = 1; i < sorteo.CantidadTicketsTotales + 1; i++)
                {
                    var ticket = new Ticket
                    {
                        SorteoId = sorteo.Id,
                        NumeroTicket = i,
                        UsuarioId = _context.Usuarios.FirstOrDefaultAsync().Result!.Id,
                        PrecioTicket = sorteo.PrecioUnidad,
                        FechaCompra = DateTime.Now,

                    };
                    await _context.Tickets.AddAsync(ticket);
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["PremioId"] = new SelectList(_context.Premios, "Id", "Descripcion", sorteo.PremioId);
            return View(sorteo);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewData["PremioId"] = new SelectList(_context.Premios, "Id", "Descripcion");
            var sorteo = await _context.Sorteos.FindAsync(id);
            if (sorteo == null)
            {
                return NotFound();
            }
            ViewData["PremioId"] = new SelectList(_context.Premios, "Id", "Id", sorteo.PremioId);
            return View(sorteo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Sorteo sorteo)
        {
            if (id != sorteo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sorteo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SorteoExists(sorteo.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["PremioId"] = new SelectList(_context.Premios, "Id", "Id", sorteo.PremioId);
            return View(sorteo);
        }

        // GET: Sorteos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sorteo = await _context.Sorteos
                .Include(s => s.Premio)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sorteo == null)
            {
                return NotFound();
            }

            return View(sorteo);
        }

        // POST: Sorteos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sorteo = await _context.Sorteos.FindAsync(id);
            if (sorteo != null)
            {
                _context.Sorteos.Remove(sorteo);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SorteoExists(int id)
        {
            return _context.Sorteos.Any(e => e.Id == id);
        }
    }
}
