using Microsoft.AspNetCore.Mvc;

public class DashboardController : Controller
{
    private readonly DashboardService _dashboardService;

    public DashboardController(DashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    public async Task<IActionResult> Index()
    {
        var inscripcionesPorCurso = await _dashboardService.ObtenerInscripcionesPorCursoAsync();

        // Log para verificar los datos
        if (inscripcionesPorCurso == null || !inscripcionesPorCurso.Any())
        {
            ViewData["Mensaje"] = "No hay datos disponibles para mostrar.";
        }
        else
        {
            // Verificar los datos antes de pasarlos a la vista
            Console.WriteLine("Datos obtenidos:");
            foreach (var item in inscripcionesPorCurso)
            {
                Console.WriteLine($"Curso: {item.NombreCurso}, Inscripciones: {item.NumeroInscripciones}");
            }
        }

        return View(inscripcionesPorCurso);
    }

}
