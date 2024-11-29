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
    public class CalificacionesController : Controller
    {
        private readonly PlataformaCursosContext _context;

        public CalificacionesController(PlataformaCursosContext context)
        {
            _context = context;
        }

        // GET: Calificaciones
        public async Task<IActionResult> Index()
        {
            var plataformaCursosContext = _context.Calificaciones.Include(c => c.IdCursoNavigation).Include(c => c.IdUsuarioNavigation);
            return View(await plataformaCursosContext.ToListAsync());
        }

        // GET: Calificaciones/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var calificacione = await _context.Calificaciones
                .Include(c => c.IdCursoNavigation)
                .Include(c => c.IdUsuarioNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (calificacione == null)
            {
                return NotFound();
            }

            return View(calificacione);
        }

        // GET: Calificaciones/Create
        public IActionResult Create()
        {
            ViewData["IdCurso"] = new SelectList(_context.Cursos, "Id", "Titulo");
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "Id", "Nombre");
            return View();
        }

        // POST: Calificaciones/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdCurso,IdUsuario,Calificacion,FechaCalificacion")] Calificacione calificacione)
        {
            if (!ModelState.IsValid)
            {
                _context.Add(calificacione);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCurso"] = new SelectList(_context.Cursos, "Id", "Id", calificacione.IdCurso);
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "Id", "Id", calificacione.IdUsuario);
            return View(calificacione);
        }

        // GET: Calificaciones/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var calificacione = await _context.Calificaciones.FindAsync(id);
            if (calificacione == null)
            {
                return NotFound();
            }
            ViewData["IdCurso"] = new SelectList(_context.Cursos, "Id", "Id", calificacione.IdCurso);
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "Id", "Id", calificacione.IdUsuario);
            return View(calificacione);
        }

        // POST: Calificaciones/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdCurso,IdUsuario,Calificacion,FechaCalificacion")] Calificacione calificacione)
        {
            if (id != calificacione.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    _context.Update(calificacione);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CalificacioneExists(calificacione.Id))
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
            ViewData["IdCurso"] = new SelectList(_context.Cursos, "Id", "Id", calificacione.IdCurso);
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "Id", "Id", calificacione.IdUsuario);
            return View(calificacione);
        }

        // GET: Calificaciones/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var calificacione = await _context.Calificaciones
                .Include(c => c.IdCursoNavigation)
                .Include(c => c.IdUsuarioNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (calificacione == null)
            {
                return NotFound();
            }

            return View(calificacione);
        }

        // POST: Calificaciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var calificacione = await _context.Calificaciones.FindAsync(id);
            if (calificacione != null)
            {
                _context.Calificaciones.Remove(calificacione);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CalificacioneExists(int id)
        {
            return _context.Calificaciones.Any(e => e.Id == id);
        }
    }
}
