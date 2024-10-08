﻿using System.ComponentModel.DataAnnotations;

namespace WebApp_Sorteo.Models
{
    public class Premio
    {
        [Key]
        public int Id { get; set; }
        public required string Descripcion { get; set; }
        public decimal Valor { get; set; }
        public required string Imagen { get; set; }
    }
}
