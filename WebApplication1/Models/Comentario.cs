using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Comentario
{
    public int Id { get; set; }

    public int IdCurso { get; set; }

    public int IdUsuario { get; set; }

    public string Contenido { get; set; } = null!;

    public DateTime? FechaComentario { get; set; }

    public virtual Curso IdCursoNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
