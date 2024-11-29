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
    public class LeccionesController : Controller
    {
        private readonly PlataformaCursosContext _context;

        public LeccionesController(PlataformaCursosContext context)
        {
            _context = context;
        }

        // GET: Lecciones
        public async Task<IActionResult> Index()
        {
            var plataformaCursosContext = _context.Lecciones.Include(l => l.IdCursoNavigation);
            return View(await plataformaCursosContext.ToListAsync());
        }

        // GET: Lecciones/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leccione = await _context.Lecciones
                .Include(l => l.IdCursoNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (leccione == null)
            {
                return NotFound();
            }

            return View(leccione);
        }

        // GET: Lecciones/Create
        public IActionResult Create()
        {
            ViewData["IdCurso"] = new SelectList(_context.Cursos, "Id", "Titulo");
            return View();
        }

        // POST: Lecciones/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdCurso,Titulo,Contenido,Orden")] Leccione leccione)
        {
            if (!ModelState.IsValid)
            {
                _context.Add(leccione);
                await _context.SaveChangesAsync();
                return RedirectToAction("Create", "Materiales", new { idCurso = leccione.IdCurso });
            }
            ViewData["IdCurso"] = new SelectList(_context.Cursos, "Id", "Id", leccione.IdCurso);
            return View(leccione);
        }

        // GET: Lecciones/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leccione = await _context.Lecciones.FindAsync(id);
            if (leccione == null)
            {
                return NotFound();
            }
            ViewData["IdCurso"] = new SelectList(_context.Cursos, "Id", "Id", leccione.IdCurso);
            return View(leccione);
        }

        // POST: Lecciones/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdCurso,Titulo,Contenido,Orden")] Leccione leccione)
        {
            if (id != leccione.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    _context.Update(leccione);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LeccioneExists(leccione.Id))
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
            ViewData["IdCurso"] = new SelectList(_context.Cursos, "Id", "Id", leccione.IdCurso);
            return View(leccione);
        }

        // GET: Lecciones/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leccione = await _context.Lecciones
                .Include(l => l.IdCursoNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (leccione == null)
            {
                return NotFound();
            }

            return View(leccione);
        }

        // POST: Lecciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var leccione = await _context.Lecciones.FindAsync(id);
            if (leccione != null)
            {
                _context.Lecciones.Remove(leccione);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LeccioneExists(int id)
        {
            return _context.Lecciones.Any(e => e.Id == id);
        }
    }
}
