using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApplication1.Models; // Asegúrate de que este espacio de nombres coincide con tu proyecto.
using System.Linq;
using Microsoft.EntityFrameworkCore;

public class ForgotPasswordController : Controller
{
    private readonly PlataformaCursosContext _context;

    public ForgotPasswordController(PlataformaCursosContext context)
    {
        _context = context;
    }

    // GET: ForgotPassword
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    // POST: ForgotPassword
    [HttpPost]
    public async Task<IActionResult> ResetPassword(string email, string newPassword)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(newPassword))
        {
            TempData["ErrorMessage"] = "Por favor, ingresa un correo y una nueva contraseña.";
            return View("Index");
        }

        // Verifica si el correo existe en la base de datos
        var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
        if (usuario == null)
        {
            TempData["ErrorMessage"] = "El correo electrónico no está registrado.";
            return View("Index");
        }

        // Actualiza la contraseña del usuario
        usuario.Password = newPassword; // Asume que la contraseña se guarda en texto plano. Cambiar a un hash seguro en producción.
        _context.Update(usuario);
        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = "Tu contraseña ha sido restablecida con éxito.";
        return RedirectToAction("Login", "Account");
    }
}
