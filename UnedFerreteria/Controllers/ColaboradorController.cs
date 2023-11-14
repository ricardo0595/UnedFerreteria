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
    public class ColaboradorController : Controller
    {
        private readonly FerreteriaContext _context;

        public ColaboradorController(FerreteriaContext context)
        {
            _context = context;
        }

        // GET: ColaboradorModels
        public async Task<IActionResult> Index()
        {
              return _context.Colaborador != null ? 
                          View(await _context.Colaborador.ToListAsync()) :
                          Problem("Entity set 'FerreteriaContext.Colaboradores'  is null.");
        }

        // GET: ColaboradorModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Colaborador == null)
            {
                return NotFound();
            }

            var colaboradorModel = await _context.Colaborador
                .FirstOrDefaultAsync(m => m.Id == id);
            if (colaboradorModel == null)
            {
                return NotFound();
            }

            return View(colaboradorModel);
        }

        // GET: ColaboradorModels/Create
        public IActionResult Create()
        {
            return View();
        }

        

        // POST: ColaboradorModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Cedula,Nombre,Apellidos,FechaRegistro,Estado")] ColaboradorModel colaboradorModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(colaboradorModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(colaboradorModel);
        }

        // GET: ColaboradorModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Colaborador == null)
            {
                return NotFound();
            }

            var colaboradorModel = await _context.Colaborador.FindAsync(id);
            if (colaboradorModel == null)
            {
                return NotFound();
            }
            return View(colaboradorModel);
        }

        // POST: ColaboradorModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Cedula,Nombre,Apellidos,FechaRegistro,Estado")] ColaboradorModel colaboradorModel)
        {
            if (id != colaboradorModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(colaboradorModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ColaboradorModelExists(colaboradorModel.Id))
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
            return View(colaboradorModel);
        }

        // GET: ColaboradorModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Colaborador == null)
            {
                return NotFound();
            }

            var colaboradorModel = await _context.Colaborador
                .FirstOrDefaultAsync(m => m.Id == id);
            if (colaboradorModel == null)
            {
                return NotFound();
            }

            return View(colaboradorModel);
        }

        // POST: ColaboradorModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Colaborador == null)
            {
                return Problem("Entity set 'FerreteriaContext.Colaboradores'  is null.");
            }
            var colaboradorModel = await _context.Colaborador.FindAsync(id);
            if (colaboradorModel != null)
            {
                _context.Colaborador.Remove(colaboradorModel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ColaboradorModelExists(int id)
        {
          return (_context.Colaborador?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
