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
    public class CursoCategoriumsController : Controller
    {
        private readonly PlataformaCursosContext _context;

        public CursoCategoriumsController(PlataformaCursosContext context)
        {
            _context = context;
        }

        // GET: CursoCategoriums
        public async Task<IActionResult> Index()
        {
            var plataformaCursosContext = _context.CursoCategoria.Include(c => c.IdCategoriaNavigation).Include(c => c.IdCursoNavigation);
            return View(await plataformaCursosContext.ToListAsync());
        }

        // GET: CursoCategoriums/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cursoCategorium = await _context.CursoCategoria
                .Include(c => c.IdCategoriaNavigation)
                .Include(c => c.IdCursoNavigation)
                .FirstOrDefaultAsync(m => m.IdCurso == id);
            if (cursoCategorium == null)
            {
                return NotFound();
            }

            return View(cursoCategorium);
        }

        // GET: CursoCategoriums/Create
        public IActionResult Create()
        {
            ViewData["IdCategoria"] = new SelectList(_context.Categorias, "Id", "Nombre");
            ViewData["IdCurso"] = new SelectList(_context.Cursos, "Id", "Titulo");
            return View();
        }

        // POST: CursoCategoriums/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdCurso,IdCategoria,FechaCreacion")] CursoCategorium cursoCategorium)
        {
            if (!ModelState.IsValid)
            {
                _context.Add(cursoCategorium);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Cursoes");
            }
            ViewData["IdCategoria"] = new SelectList(_context.Categorias, "Id", "Id", cursoCategorium.IdCategoria);
            ViewData["IdCurso"] = new SelectList(_context.Cursos, "Id", "Id", cursoCategorium.IdCurso);
            return View(cursoCategorium);
        }

        // GET: CursoCategoriums/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cursoCategorium = await _context.CursoCategoria.FindAsync(id);
            if (cursoCategorium == null)
            {
                return NotFound();
            }
            ViewData["IdCategoria"] = new SelectList(_context.Categorias, "Id", "Id", cursoCategorium.IdCategoria);
            ViewData["IdCurso"] = new SelectList(_context.Cursos, "Id", "Id", cursoCategorium.IdCurso);
            return View(cursoCategorium);
        }

        // POST: CursoCategoriums/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdCurso,IdCategoria,FechaCreacion")] CursoCategorium cursoCategorium)
        {
            if (id != cursoCategorium.IdCurso)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    _context.Update(cursoCategorium);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CursoCategoriumExists(cursoCategorium.IdCurso))
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
            ViewData["IdCategoria"] = new SelectList(_context.Categorias, "Id", "Id", cursoCategorium.IdCategoria);
            ViewData["IdCurso"] = new SelectList(_context.Cursos, "Id", "Id", cursoCategorium.IdCurso);
            return View(cursoCategorium);
        }

        // GET: CursoCategoriums/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cursoCategorium = await _context.CursoCategoria
                .Include(c => c.IdCategoriaNavigation)
                .Include(c => c.IdCursoNavigation)
                .FirstOrDefaultAsync(m => m.IdCurso == id);
            if (cursoCategorium == null)
            {
                return NotFound();
            }

            return View(cursoCategorium);
        }

        // POST: CursoCategoriums/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cursoCategorium = await _context.CursoCategoria.FindAsync(id);
            if (cursoCategorium != null)
            {
                _context.CursoCategoria.Remove(cursoCategorium);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CursoCategoriumExists(int id)
        {
            return _context.CursoCategoria.Any(e => e.IdCurso == id);
        }
    }
}
