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
    public class SoportesController : Controller
    {
        private readonly PlataformaCursosContext _context;

        public SoportesController(PlataformaCursosContext context)
        {
            _context = context;
        }

        // GET: Soportes
        public async Task<IActionResult> Index()
        {
            var plataformaCursosContext = _context.Soportes.Include(s => s.IdUsuarioNavigation);
            return View(await plataformaCursosContext.ToListAsync());
        }

        // GET: Soportes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var soporte = await _context.Soportes
                .Include(s => s.IdUsuarioNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (soporte == null)
            {
                return NotFound();
            }

            return View(soporte);
        }

        // GET: Soportes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Soportes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdUsuario,Mensaje,FechaMensaje,Estado")] Soporte soporte)
        {
            if (ModelState.IsValid)
            {
                _context.Add(soporte);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            return View(soporte);
        }

        // GET: Soportes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var soporte = await _context.Soportes.FindAsync(id);
            if (soporte == null)
            {
                return NotFound();
            }

            // Cargar la lista de usuarios y el estado en ViewBag
            ViewBag.IdUsuario = new SelectList(await _context.Usuarios.ToListAsync(), "Id", "Nombre", soporte.IdUsuario);
            ViewBag.EstadoOptions = new SelectList(new[] { "Pendiente", "Completado" }, soporte.Estado);

            return View(soporte);
        }


        // POST: Soportes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdUsuario,Mensaje,FechaMensaje,Estado")] Soporte soporte)
        {
            if (id != soporte.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Console.WriteLine($"IdUsuario: {soporte.IdUsuario}, Estado: {soporte.Estado}, Mensaje: {soporte.Mensaje}, FechaMensaje: {soporte.FechaMensaje}");

                    _context.Update(soporte);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SoporteExists(soporte.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

            }
            return View(soporte);
        }


        // GET: Soportes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var soporte = await _context.Soportes
                .Include(s => s.IdUsuarioNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (soporte == null)
            {
                return NotFound();
            }
            ViewBag.EstadoOptions = new SelectList(new[] { "Pendiente", "Completado" });
            ViewBag.IdUsuario = new SelectList(await _context.Usuarios.ToListAsync(), "Id", "Nombre", soporte.IdUsuario);

            return View(soporte);
        }

        // POST: Soportes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var soporte = await _context.Soportes.FindAsync(id);
            if (soporte != null)
            {
                _context.Soportes.Remove(soporte);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SoporteExists(int id)
        {
            return _context.Soportes.Any(e => e.Id == id);
        }
    }
}
