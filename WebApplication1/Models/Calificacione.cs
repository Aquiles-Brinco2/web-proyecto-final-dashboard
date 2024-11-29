using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Calificacione
{
    public int Id { get; set; }

    public int IdCurso { get; set; }

    public int IdUsuario { get; set; }

    public int? Calificacion { get; set; }

    public DateTime? FechaCalificacion { get; set; }

    public virtual Curso IdCursoNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
