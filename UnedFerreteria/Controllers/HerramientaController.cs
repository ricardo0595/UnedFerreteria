using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UnedFerreteria.Models;

namespace UnedFerreteria.Controllers
{
    public class HerramientaController : Controller
    {
        private readonly FerreteriaContext _context;

        public HerramientaController(FerreteriaContext context)
        {
            _context = context;
        }

        // GET: Herramienta
        public async Task<IActionResult> Index()
        {
              return _context.Herramienta != null ? 
                          View(await _context.Herramienta.ToListAsync()) :
                          Problem("Entity set 'FerreteriaContext.Herramienta'  is null.");
        }

      

        // GET: Herramienta/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Herramienta == null)
            {
                return NotFound();
            }

            var herramientaModel = await _context.Herramienta
                .FirstOrDefaultAsync(m => m.Id == id);
            if (herramientaModel == null)
            {
                return NotFound();
            }

            return View(herramientaModel);
        }

        // GET: Herramienta/Create
        public IActionResult Create()
        {
            return View();
        }

        public bool CodeAlreadyExists(string code)
        {
            try
            {
                if (_context.Herramienta?.FirstOrDefault(e => e.Codigo == code) != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                return false;
                throw;
            }
           
        }

        // POST: Herramienta/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Descripcion,Cantidad,Codigo")] HerramientaModel herramientaModel)
        {
            if (ModelState.IsValid)
            {
                if(herramientaModel.Errores!=null && herramientaModel.Errores.Count > 0)
                {
                    herramientaModel.Errores.Clear();
                }

                if (this.CodeAlreadyExists(herramientaModel.Codigo))
                {
                    herramientaModel.Errores = new List<ErrorVistaModel>();
                    herramientaModel.Errores.Add(new ErrorVistaModel { Titulo = "Codigo existente", Descripcion = "El codigo ingresado ya existe" });
                    return View(herramientaModel);
                }
                else
                {
                    _context.Add(herramientaModel);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));

                }
            }
            return View(herramientaModel);
        }

        // GET: Herramienta/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Herramienta == null)
            {
                return NotFound();
            }

            var herramientaModel = await _context.Herramienta.FindAsync(id);
            if (herramientaModel == null)
            {
                return NotFound();
            }
            return View(herramientaModel);
        }

        // POST: Herramienta/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Descripcion,Cantidad,Codigo")] HerramientaModel herramientaModel)
        {
            if (id != herramientaModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(herramientaModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HerramientaModelExists(herramientaModel.Id))
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
            return View(herramientaModel);
        }

        // GET: Herramienta/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Herramienta == null)
            {
                return NotFound();
            }

            var herramientaModel = await _context.Herramienta
                .FirstOrDefaultAsync(m => m.Id == id);
            if (herramientaModel == null)
            {
                return NotFound();
            }

            return View(herramientaModel);
        }

        // POST: Herramienta/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Herramienta == null)
            {
                return Problem("Entity set 'FerreteriaContext.Herramienta'  is null.");
            }
            var herramientaModel = await _context.Herramienta.FindAsync(id);
            if (herramientaModel != null)
            {
                _context.Herramienta.Remove(herramientaModel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HerramientaModelExists(int id)
        {
          return (_context.Herramienta?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        [HttpGet]
        public JsonResult GetHerramientasByColaborador(int idColaborador)
        {
            List<int> listaIds = _context.Prestamos.Where(p => p.IdColaborador == idColaborador).Select(p => p.IdHerramienta).Distinct().ToList();
            List<HerramientaModel> list = new List<HerramientaModel>();

            foreach (var id in listaIds)
            {
                list.Add(new HerramientaModel {Id = _context.Herramienta.Find(id).Id , Nombre = _context.Herramienta.Find(id).Nombre });
            }
            return Json(list);
        }

    }
}
