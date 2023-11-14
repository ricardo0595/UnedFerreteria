using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace UnedFerreteria.Models
{
    public class PrestamosModel
    {
        public int Id { get; set; }
        public int IdColaborador { get; set; }
        public int IdHerramienta { get; set; }
        public DateTime FechaPrestamo { get; set; }
        public DateTime FechaEsperada { get; set; }
        public DateTime? FechaEntrega { get; set; }
        [ValidateNever]
        [NotMapped]
        public List<ErrorVistaModel> Errores { get; set; }

    }
}
