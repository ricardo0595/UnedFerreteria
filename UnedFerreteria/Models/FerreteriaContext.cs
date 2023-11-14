using Microsoft.EntityFrameworkCore;
using UnedFerreteria.Models;

namespace UnedFerreteria.Models
{
    public class FerreteriaContext : DbContext
    {
        public DbSet<ColaboradorModel> Colaborador { get; set; }
        public DbSet<UnedFerreteria.Models.HerramientaModel>? Herramienta { get; set; }

        public FerreteriaContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<UnedFerreteria.Models.PrestamosModel>? Prestamos { get; set; }

    }
}
