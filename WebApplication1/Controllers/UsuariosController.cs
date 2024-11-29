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
    public class UsuariosController : Controller
    {
        private readonly PlataformaCursosContext _context;

        public UsuariosController(PlataformaCursosContext context)
        {
            _context = context;
        }

        // GET: Usuarios
        public async Task<IActionResult> Index()
        {
            return View(await _context.Usuarios.ToListAsync());
        }



            // Acción para mostrar la lista de instructores
            public IActionResult Profesores()
            {
                // Obtener todos los usuarios que sean instructores
                var instructores = _context.Usuarios
                                           .Where(u => u.Tipo == "instructor")
                                           .ToList();

                // Enviar la lista de instructores a la vista
                return View(instructores);
            }

        public IActionResult Estudiantes()
        {
            // Obtener todos los usuarios que sean instructores
            var estudiantes = _context.Usuarios
                                       .Where(u => u.Tipo == "estudiante")
                                       .ToList();

            // Enviar la lista de instructores a la vista
            return View(estudiantes);
        }




        // GET: Usuarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // GET: Usuarios/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Usuarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Email,Password,Tipo,FechaRegistro,Imagen")] Usuario usuario, IFormFile Imagen)
        {
            if (ModelState.IsValid)
            {
                // Verificar si se ha subido una imagen
                if (Imagen != null && Imagen.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        // Leer el archivo de imagen en el MemoryStream
                        await Imagen.CopyToAsync(memoryStream);

                        // Asignar el arreglo de bytes de la imagen al campo Imagen del modelo
                        usuario.Imagen = memoryStream.ToArray();
                    }
                }

                _context.Add(usuario);
                await _context.SaveChangesAsync();

                TempData["Message"] = "Usuario creado con éxito!";
                return RedirectToAction(nameof(Index));
            }
            return View(usuario);
        }


        // GET: Usuarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }

        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Email,Password,Tipo,FechaRegistro,Imagen")] Usuario usuario, IFormFile Imagen)
        {
            if (id != usuario.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Verificar si se ha subido una nueva imagen
                    if (Imagen != null && Imagen.Length > 0)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await Imagen.CopyToAsync(memoryStream);  // Copiar la imagen a memoria
                            usuario.Imagen = memoryStream.ToArray();  // Convertir la imagen a un arreglo de bytes
                        }
                    }

                    // Actualizar el usuario en la base de datos
                    _context.Update(usuario);
                    await _context.SaveChangesAsync();

                    TempData["Message"] = "Usuario actualizado con éxito!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    TempData["Message"] = "Error al actualizar. Inténtalo nuevamente.";
                    if (!UsuarioExists(usuario.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(usuario);
        }


        // GET: Usuarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.Id == id);
        }
    }
}
