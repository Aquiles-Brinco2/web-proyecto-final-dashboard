using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class MaterialesController : Controller
    {
        private readonly PlataformaCursosContext _context;

        public MaterialesController(PlataformaCursosContext context)
        {
            _context = context;
        }

        // GET: Materiales
        public async Task<IActionResult> Index()
        {
            var plataformaCursosContext = _context.Materiales.Include(m => m.IdCursoNavigation);
            return View(await plataformaCursosContext.ToListAsync());
        }

        // GET: Materiales/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var materiale = await _context.Materiales
                .Include(m => m.IdCursoNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (materiale == null)
            {
                return NotFound();
            }

            return View(materiale);
        }

        // GET: Materiales/Create
        public IActionResult Create()
        {
            ViewData["IdCurso"] = new SelectList(_context.Cursos, "Id", "Titulo");
            return View();
        }

        // POST: Materiales/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdCurso,Tipo,Url")] Materiale materiale)
        {
            if (!ModelState.IsValid)
            {
                _context.Add(materiale);
                await _context.SaveChangesAsync();
                return RedirectToAction("Create", "CursoCategoriums", new { idCurso = materiale.IdCurso });
            }
            ViewData["IdCurso"] = new SelectList(_context.Cursos, "Id", "Id", materiale.IdCurso);
            return View(materiale);
        }

        // GET: Materiales/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var materiale = await _context.Materiales.FindAsync(id);
            if (materiale == null)
            {
                return NotFound();
            }
            ViewData["IdCurso"] = new SelectList(_context.Cursos, "Id", "Id", materiale.IdCurso);
            return View(materiale);
        }

        // POST: Materiales/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdCurso,Tipo,Url")] Materiale materiale)
        {
            if (id != materiale.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    _context.Update(materiale);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MaterialeExists(materiale.Id))
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
            ViewData["IdCurso"] = new SelectList(_context.Cursos, "Id", "Id", materiale.IdCurso);
            return View(materiale);
        }

        // GET: Materiales/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var materiale = await _context.Materiales
                .Include(m => m.IdCursoNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (materiale == null)
            {
                return NotFound();
            }

            return View(materiale);
        }

        // POST: Materiales/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var materiale = await _context.Materiales.FindAsync(id);
            if (materiale != null)
            {
                _context.Materiales.Remove(materiale);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MaterialeExists(int id)
        {
            return _context.Materiales.Any(e => e.Id == id);
        }
    }
}
