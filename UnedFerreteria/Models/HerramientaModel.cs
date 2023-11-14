using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UnedFerreteria.Models
{

    public class HerramientaModel
    {
        public int Id { get; set; }
        [Required]
        public String? Nombre { get; set; }
        [Required]
        public String? Descripcion { get; set; }
        [Required]
        public int? Cantidad { get; set; }
        [Required]
        [MaxLength(4)]
        public String? Codigo { get; set; }
        [ValidateNever]
        [NotMapped]
        public List<ErrorVistaModel> Errores { get; set; }
    }
}
