using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;  // Ajusta el namespace según tu proyecto
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

public class AccountController : Controller
{
    private readonly PlataformaCursosContext _context;

    public AccountController(PlataformaCursosContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(string email, string password)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            ViewData["ErrorMessage"] = "Por favor, completa todos los campos.";
            return View();
        }

        // Simulación de autenticación, aquí deberías colocar tu lógica real
        var user = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email && u.Password == password);

        if (user != null)
        {
            HttpContext.Session.SetString("UserId", user.Id.ToString());
            HttpContext.Session.SetString("UserName", user.Nombre);
            HttpContext.Session.SetString("UserType", user.Tipo); // Asumimos que este campo es "Estudiante", "Administrador" o "Profesor"
            return RedirectToAction("Index", "Home");
        }

        // Si la autenticación falla, enviar el mensaje de error a la vista
        ViewData["ErrorMessage"] = "Correo o contraseña incorrectos. Inténtalo de nuevo.";
        return View();
    }


    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }
}
