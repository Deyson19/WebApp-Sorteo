

using System.ComponentModel.DataAnnotations;

namespace WebApp_Sorteo.Models
{
    public class EnviarPago
    {
        public required Ticket Ticket { get; set; }
        public required string PersonType { get; set; }
        public required string IdentificationType { get; set; }
        public required string IdentificationNumber { get; set; }
        // public required string BanksList { get; set; }
        // public required string TransactionAmount { get; set; }
        // public required string Description { get; set; }
        // public required string ZipCode { get; set; }
        // public required string StreetName { get; set; }
        // public required string StreetNumber { get; set; }
        // public required string Neighborhood { get; set; }
        // public required string City { get; set; }
        // public required string FederalUnit { get; set; }
        // public required string PhoneAreaCode { get; set; }
    }
    public class EnviarPagoDTO
    {
        //Datos para solicitar al usuario en el formulario para procesar el pago con mercado pago
        [Display(Name = "Correo")]
        public required string Email { get; set; }
        [Display(Name = "Número Telefónico")]
        public required string PhoneNumber { get; set; }
        [Display(Name = "Tipo de Persona")]
        public required string PersonType { get; set; }
        [Display(Name = "Tipo de Identificación")]
        public string IdentificationType { get; set; }
        [Display(Name = "Número de Identificación")]
        public required string IdentificationNumber { get; set; }
        [Display(Name = "Código Postal")]
        public required string ZipCode { get; set; }
        [Display(Name = "Nombre de la calle")]
        public required string StreetName { get; set; }
        [Display(Name = "Número de la calle")]
        public required string StreetNumber { get; set; }
        [Display(Name = "Barrio")]
        public required string Neighborhood { get; set; }
        [Display(Name = "Ciudad")]
        public required string City { get; set; }
        public required string PhoneAreaCode { get; set; }
    }
}