using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UnedFerreteria.Models;

namespace UnedFerreteria.Controllers
{
    public class PrestamosController : Controller
    {
        private readonly FerreteriaContext _context;

        public PrestamosController(FerreteriaContext context)
        {
            _context = context;
        }

        // GET: Prestamos
        public async Task<IActionResult> Index()
        {
              return _context.Prestamos != null ? 
                          View(await _context.Prestamos.ToListAsync()) :
                          Problem("Entity set 'FerreteriaContext.PrestamosModel'  is null.");
        }

        // GET: Prestamos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Prestamos == null)
            {
                return NotFound();
            }

            var prestamosModel = await _context.Prestamos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (prestamosModel == null)
            {
                return NotFound();
            }

            return View(prestamosModel);
        }

        // GET: Prestamos/Create
        public IActionResult Create()
        {
            List<HerramientaModel> listaHerramienta = _context.Herramienta.ToList();
            List<ColaboradorModel> listaColaborador = _context.Colaborador.ToList();

            ViewData["listaHerramientas"] = listaHerramienta;
            ViewData["listaColaborador"] = listaColaborador;

            return View();
        }

        // GET: Prestamos/Create
        public IActionResult Devolver()
        {
            List<HerramientaModel> listaHerramienta = _context.Herramienta.ToList();
            List<ColaboradorModel> listaColaborador = _context.Colaborador.ToList();

            ViewData["listaHerramientas"] = listaHerramienta;
            ViewData["listaColaborador"] = listaColaborador;

            return View();
        }

        // POST: Prestamos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdColaborador,IdHerramienta,FechaPrestamo,FechaEsperada,FechaEntrega")] PrestamosModel prestamosModel)
        {
            if (ModelState.IsValid)
            {
                var validaciones = this.Validaciones(prestamosModel);

                if (prestamosModel.Errores != null && prestamosModel.Errores.Count > 0)
                {
                    prestamosModel.Errores.Clear();
                }

                
                if (validaciones.Count > 0)
                {
                    List<HerramientaModel> listaHerramienta = _context.Herramienta.ToList();
                    List<ColaboradorModel> listaColaborador = _context.Colaborador.ToList();

                    ViewData["listaHerramientas"] = listaHerramienta;
                    ViewData["listaColaborador"] = listaColaborador;
                    prestamosModel.Errores = validaciones;
                    return View(prestamosModel);
                }
                else
                {
                    prestamosModel.FechaEntrega = null;
                    _context.Add(prestamosModel);
                    _context.Herramienta.Where(c => c.Id == prestamosModel.IdHerramienta).ExecuteUpdate(setter => setter.SetProperty(e => e.Cantidad, e => e.Cantidad - 1));
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
              
            }
            return View(prestamosModel);
        }

        // GET: Prestamos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Prestamos == null)
            {
                return NotFound();
            }

            var prestamosModel = await _context.Prestamos.FindAsync(id);
            if (prestamosModel == null)
            {
                return NotFound();
            }

            List<ColaboradorModel> listaColaborador = _context.Colaborador.ToList();
            ViewData["listaColaborador"] = listaColaborador;

            return View(prestamosModel);
        }

        // POST: Prestamos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdColaborador,IdHerramienta,FechaPrestamo,FechaEsperada,FechaEntrega")] PrestamosModel prestamosModel)
        {
            if (id != prestamosModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(prestamosModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PrestamosModelExists(prestamosModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(prestamosModel);
        }

        // GET: Prestamos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Prestamos == null)
            {
                return NotFound();
            }

            var prestamosModel = await _context.Prestamos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (prestamosModel == null)
            {
                return NotFound();
            }

            return View(prestamosModel);
        }

        // POST: Prestamos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Prestamos == null)
            {
                return Problem("Entity set 'FerreteriaContext.PrestamosModel'  is null.");
            }
            var prestamosModel = await _context.Prestamos.FindAsync(id);
            if (prestamosModel != null)
            {
                _context.Prestamos.Remove(prestamosModel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PrestamosModelExists(int id)
        {
          return (_context.Prestamos?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public List<ErrorVistaModel> Validaciones(PrestamosModel model)
        {
            List<ErrorVistaModel> errores = new List<ErrorVistaModel>();
            int totalHerramientasColaborador = _context.Prestamos.Where(p => p.IdColaborador == model.IdColaborador).Count();
            
            if (_context.Herramienta.Find(model.IdHerramienta).Cantidad <= 0)
            {
                errores.Add(new ErrorVistaModel { Titulo = "Fuera de inventario", Descripcion = "fuera de inventario" });
            }else if (totalHerramientasColaborador >= 5)
            {
                errores.Add(new ErrorVistaModel { Titulo = "Maximo de herramientas por Colaborador", Descripcion = "Este colaborador ya ha alcanzado el limite de herramientas permitido (5)" });
            }else if (model.FechaEsperada > model.FechaPrestamo.AddDays(5))
            {
                errores.Add(new ErrorVistaModel { Titulo = "Limite de dias de prestamo", Descripcion = "La fecha de entrega de la herramienta no debe ser mayor a 5 dias" });
            }

            return errores;
        }

    }
}
