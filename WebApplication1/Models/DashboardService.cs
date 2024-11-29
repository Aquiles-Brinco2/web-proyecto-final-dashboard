using WebApplication1.Models;
using Microsoft.EntityFrameworkCore;

public class DashboardService
{
    private readonly PlataformaCursosContext _context;

    public DashboardService(PlataformaCursosContext context)
    {
        _context = context;
    }

    public async Task<List<InscripcionPorCurso>> ObtenerInscripcionesPorCursoAsync()
    {
        // Realiza la consulta sobre la base de datos, asegurándote de que sea asincrónica
        var inscripciones = await _context.Inscripciones
            .GroupBy(i => i.IdCurso)
            .Select(g => new InscripcionPorCurso
            {
                NombreCurso = _context.Cursos
                    .Where(c => c.Id == g.Key)
                    .Select(c => c.Titulo)
                    .FirstOrDefault(),
                NumeroInscripciones = g.Count()
            })
            .ToListAsync(); // Esto debe funcionar si estás utilizando Entity Framework Core

        return inscripciones;
    }
}
