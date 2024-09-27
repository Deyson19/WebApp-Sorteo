using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp_Sorteo.Models
{
    public class Usuario : IdentityUser
    {
        [Required(ErrorMessage = "{0} es requerido")]
        [MaxLength(80)]
        public required string Nombres { get; set; }

        [Required(ErrorMessage = "{0} es requerido")]
        [MaxLength(80)]
        public required string Apellidos { get; set; }

        public required string Documento { get; set; }

        [Required(ErrorMessage = "{0} es requerido")]
        [MaxLength(200)]
        public required string Direccion { get; set; }

        [Required(ErrorMessage = "{0} es requerido")]
        [MaxLength(80)]
        public required string Ciudad { get; set; }
        public string Estado { get; set; }

        [Required(ErrorMessage = "{0} es requerido")]
        [MaxLength(60)]
        public required string Pais { get; set; }
        [NotMapped]
        public string? Role { get; set; }
    }
}
