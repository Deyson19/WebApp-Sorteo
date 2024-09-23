using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApp_Sorteo.Helpers;

namespace WebApp_Sorteo.Models
{
    public class Pago
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("TicketId")]
        public int TicketId { get; set; }
        public Ticket? Ticket { get; set; }
        public DateTime FechaPago { get; set; }
        public decimal Monto { get; set; }
        public required string MetodoPago { get; set; }
        public EstadoPago EstadoPago { get; set; }
        public required string DetallesPago { get; set; }

    }
}
