using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public class MisCursosViewModel
{
    public int CursoId { get; set; }
    public string TituloCurso { get; set; }
    public string DescripcionCurso { get; set; }
    public DateTime? FechaInscripcion { get; set; } // Ahora es nullable
    public int? Calificacion { get; set; } // Ahora es nullable
}
