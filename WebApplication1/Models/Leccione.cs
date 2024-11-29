using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Leccione
{
    public int Id { get; set; }

    public int IdCurso { get; set; }

    public string Titulo { get; set; } = null!;

    public string Contenido { get; set; } = null!;

    public int Orden { get; set; }

    public virtual Curso IdCursoNavigation { get; set; } = null!;
}
