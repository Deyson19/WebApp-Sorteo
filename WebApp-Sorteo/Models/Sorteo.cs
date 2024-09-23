using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApp_Sorteo.Helpers;

namespace WebApp_Sorteo.Models
{
    public class Sorteo
    {
        [Key]
        public int Id { get; set; }
        public required string Nombre { get; set; }
        public required string Descripcion { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public EstadoSorteo Estado { get; set; } = EstadoSorteo.Activo;
        public int CantidadTicketsTotales { get; set; }
        public int CantidadTicketsVendidos { get; set; }
        public double PrecioUnidad { get; set; }
        [ForeignKey("PremioId")]
        public int PremioId { get; set; }
        public Premio? Premio { get; set; }
        public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
}
