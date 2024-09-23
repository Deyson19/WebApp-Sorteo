using System.ComponentModel.DataAnnotations;
using WebApp_Sorteo.Helpers;

namespace WebApp_Sorteo.Models
{
    public class Ticket
    {
        [Key]
        public int Id { get; set; }
        public int NumeroTicket { get; set; }
        public DateTime FechaCompra { get; set; }
        public EstadoPago EstadoPago { get; set; }
        public decimal MontoPagado { get; set; }
        public double PrecioTicket { get; set; }
        public int SorteoId { get; set; }
        public Sorteo? Sorteo { get; set; }
        public required string UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }
    }
}
