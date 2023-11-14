using System.ComponentModel.DataAnnotations;

namespace UnedFerreteria.Models
{
    public class ColaboradorModel
    {
        
        public int Id { get; set; }
        [Required]
        public String? Cedula { get; set; }
        [Required]
        public String? Nombre { get; set; }
        [Required]
        public String? Apellidos { get; set; }
        [Required]
        public DateTime? FechaRegistro { get; set; }
        [Required]
        public Boolean Estado { get; set; }
    }
}
