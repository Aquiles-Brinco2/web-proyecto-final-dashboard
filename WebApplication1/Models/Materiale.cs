using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Materiale
{
    public int Id { get; set; }

    public int IdCurso { get; set; }

    public string Tipo { get; set; } = null!;

    public string Url { get; set; } = null!;

    public virtual Curso IdCursoNavigation { get; set; } = null!;
}
