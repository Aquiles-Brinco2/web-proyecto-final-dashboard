using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class RegisterController : Controller
    {
        private readonly PlataformaCursosContext _context;

        public RegisterController(PlataformaCursosContext context)
        {
            _context = context;
        }

        // GET: Register/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nombre,Email,Password,Tipo")] Usuario usuario) // Elimina Tipo del Bind
        {
            if (ModelState.IsValid)
            {
                // Verificar si el correo ya existe
                if (_context.Usuarios.Any(u => u.Email == usuario.Email))
                {
                    ModelState.AddModelError("Email", "El correo electrónico ya está en uso.");
                    return View(usuario);
                }

                // Asignar el valor predeterminado para Tipo
                usuario.Tipo = "estudiante";
                usuario.FechaRegistro = DateTime.Now; // O cualquier lógica que quieras usar
                _context.Add(usuario);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Creado con éxito";
                return RedirectToAction("Login", "Account"); // Cambia "Account" al controlador correcto para login
            }

            return View(usuario);
        }

    }
}
