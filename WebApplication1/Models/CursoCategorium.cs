using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class CursoCategorium
{
    public int IdCurso { get; set; }

    public int IdCategoria { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public virtual Categoria IdCategoriaNavigation { get; set; } = null!;

    public virtual Curso IdCursoNavigation { get; set; } = null!;
}
