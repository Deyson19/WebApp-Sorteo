using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp_Sorteo.Data;
using WebApp_Sorteo.Helpers;
using WebApp_Sorteo.Models;

namespace WebApp_Sorteo.Controllers
{
    public class TicketsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TicketsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Tickets
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Tickets.Include(t => t.Sorteo).Include(t => t.Usuario);
            var disponibles = await applicationDbContext.Where(x => x.Sorteo!.Estado == Helpers.EstadoSorteo.Activo && x.EstadoPago != Helpers.EstadoPago.Pagado).ToListAsync();
            return View(disponibles);
        }

        // GET: Tickets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets
                .Include(t => t.Sorteo)
                .Include(t => t.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // GET: Tickets/Create
        public IActionResult Create()
        {
            ViewData["SorteoId"] = new SelectList(_context.Sorteos, "Id", "Id");
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Id");
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NumeroTicket,FechaCompra,EstadoPago,MontoPagado,SorteoId,UsuarioId")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ticket);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SorteoId"] = new SelectList(_context.Sorteos, "Id", "Id", ticket.SorteoId);
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Id", ticket.UsuarioId);
            return View(ticket);
        }
        [HttpGet]
        public async Task<IActionResult> Elegir(int id)
        {
            var ticket = await _context.Tickets.FirstOrDefaultAsync(x => x.Id == id);
            if (ticket == null)
            {
                return View("Index");
            }
            var userLogged = await GetCurrentUser();
            if (userLogged == null)
            {
                return View("Index");
            }
            ticket.UsuarioId = userLogged.Id;
            ticket.EstadoPago = EstadoPago.Pendiente;
            await _context.SaveChangesAsync();
            return Ok(new
            {
                message = "Todo ok",
                ticket = ticket
            });
        }

        private async Task<Usuario> GetCurrentUser()
        {
            return await _context.Usuarios.FirstOrDefaultAsync(x => x.UserName == User.Identity.Name);
        }
        // GET: Tickets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }
            ViewData["SorteoId"] = new SelectList(_context.Sorteos, "Id", "Id", ticket.SorteoId);
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Id", ticket.UsuarioId);
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Ticket ticket)
        {
            if (id != ticket.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ticket);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketExists(ticket.Id))
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
            ViewData["SorteoId"] = new SelectList(_context.Sorteos, "Id", "Id", ticket.SorteoId);
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Id", ticket.UsuarioId);
            return View(ticket);
        }

        // GET: Tickets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets
                .Include(t => t.Sorteo)
                .Include(t => t.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket != null)
            {
                _context.Tickets.Remove(ticket);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TicketExists(int id)
        {
            return _context.Tickets.Any(e => e.Id == id);
        }
    }
}
