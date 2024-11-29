using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public static class SessionExtensions
    {
        // Método para guardar objetos en la sesión
        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        // Método para recuperar objetos de la sesión
        public static T GetObjectFromJson<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }
    }



    public class InscripcionesController : Controller
    {
        private readonly PlataformaCursosContext _context;

        public InscripcionesController(PlataformaCursosContext context)
        {
            _context = context;
        }


        // Acción para obtener la cantidad de elementos en el carrito
        public int GetCartItemCount()
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<Inscripcione>>("Cart") ?? new List<Inscripcione>();
            return cart.Count;  // Devuelve el número de elementos en el carrito
        }

        // Acción para agregar al carrito
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddToCart([Bind("Id,IdCurso,FechaInscripcion,Estado")] Inscripcione inscripcione)
        {
            // Obtener el IdUsuario desde la sesión
            var userId = HttpContext.Session.GetString("UserId");

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            // Asignar el IdUsuario
            inscripcione.IdUsuario = int.Parse(userId);

            // Recuperar el carrito de la sesión, si existe
            List<Inscripcione> cart = HttpContext.Session.GetObjectFromJson<List<Inscripcione>>("Cart") ?? new List<Inscripcione>();

            // Agregar la inscripción al carrito
            cart.Add(inscripcione);

            // Guardar el carrito en la sesión
            HttpContext.Session.SetObjectAsJson("Cart", cart);

            // Mensaje para mostrar que se agregó al carrito
            TempData["Message"] = "La inscripción se ha agregado al carrito.";

            // Redirigir de vuelta a la pantalla de agregar inscripción
            return RedirectToAction("Create");
        }

        // Acción para ver el carrito
        // Acción para ver el carrito
        public IActionResult Cart()
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<Inscripcione>>("Cart") ?? new List<Inscripcione>();

            // Obtener los nombres de los usuarios y cursos
            var cartViewModel = cart.Select(inscripcion => new CarritoViewModel
            {
                Id = inscripcion.Id,
                NombreUsuario = _context.Usuarios.FirstOrDefault(u => u.Id == inscripcion.IdUsuario)?.Nombre, // Obtener nombre de usuario
                NombreCurso = _context.Cursos.FirstOrDefault(c => c.Id == inscripcion.IdCurso)?.Titulo, // Obtener nombre del curso
            }).ToList();

            return View(cartViewModel);
        }


        // Acción para crear todas las inscripciones del carrito
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateFromCart()
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<Inscripcione>>("Cart") ?? new List<Inscripcione>();

            // Si el carrito está vacío, redirigir
            if (cart.Count == 0)
            {
                TempData["Message"] = "El carrito está vacío.";
                return RedirectToAction("Cart");
            }

            // Agregar todas las inscripciones al contexto y guardar
            _context.AddRange(cart);
            await _context.SaveChangesAsync();

            // Limpiar el carrito después de crear las inscripciones
            HttpContext.Session.Remove("Cart");

            TempData["Message"] = "Las inscripciones han sido creadas exitosamente.";

            return RedirectToAction("Index");
        }




        // GET: Inscripciones
        public async Task<IActionResult> Index()
        {

            var cartItemCount = HttpContext.Session.GetObjectFromJson<List<Inscripcione>>("Cart")?.Count ?? 0;
            TempData["CartItemCount"] = cartItemCount;

            var plataformaCursosContext = _context.Inscripciones.Include(i => i.IdCursoNavigation).Include(i => i.IdUsuarioNavigation);
            return View(await plataformaCursosContext.ToListAsync());
        }

        // GET: Inscripciones/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inscripcione = await _context.Inscripciones
                .Include(i => i.IdCursoNavigation)
                .Include(i => i.IdUsuarioNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (inscripcione == null)
            {
                return NotFound();
            }

            return View(inscripcione);
        }

        // GET: Inscripciones/Create
        public IActionResult Create()
        {
            ViewData["IdCurso"] = new SelectList(_context.Cursos, "Id", "Titulo");
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "Id", "Id");
            return View();
        }

        // POST: Inscripciones/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdUsuario,IdCurso,FechaInscripcion,Estado")] Inscripcione inscripcione)
        {
            if (!ModelState.IsValid)
            {
                _context.Add(inscripcione);
                await _context.SaveChangesAsync();
                return RedirectToAction("Create", "Pagoes");
            }
            ViewData["IdCurso"] = new SelectList(_context.Cursos, "Id", "Id", inscripcione.IdCurso);
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "Id", "Id", inscripcione.IdUsuario);
            return View(inscripcione);
        }

        // GET: Inscripciones/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inscripcione = await _context.Inscripciones.FindAsync(id);
            if (inscripcione == null)
            {
                return NotFound();
            }
            ViewData["IdCurso"] = new SelectList(_context.Cursos, "Id", "Id", inscripcione.IdCurso);
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "Id", "Id", inscripcione.IdUsuario);
            return View(inscripcione);
        }

        // POST: Inscripciones/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdUsuario,IdCurso,FechaInscripcion,Estado")] Inscripcione inscripcione)
        {
            if (id != inscripcione.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    _context.Update(inscripcione);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InscripcioneExists(inscripcione.Id))
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
            ViewData["IdCurso"] = new SelectList(_context.Cursos, "Id", "Id", inscripcione.IdCurso);
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "Id", "Id", inscripcione.IdUsuario);
            return View(inscripcione);
        }

        // GET: Inscripciones/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inscripcione = await _context.Inscripciones
                .Include(i => i.IdCursoNavigation)
                .Include(i => i.IdUsuarioNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (inscripcione == null)
            {
                return NotFound();
            }

            return View(inscripcione);
        }

        // POST: Inscripciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var inscripcione = await _context.Inscripciones.FindAsync(id);
            if (inscripcione != null)
            {
                _context.Inscripciones.Remove(inscripcione);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InscripcioneExists(int id)
        {
            return _context.Inscripciones.Any(e => e.Id == id);
        }
    }
}
