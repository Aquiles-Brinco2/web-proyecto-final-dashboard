using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Inscripcione
{
    public int Id { get; set; }

    public int IdUsuario { get; set; }

    public int IdCurso { get; set; }

    public DateTime? FechaInscripcion { get; set; }

    public string? Estado { get; set; }

    public virtual Curso IdCursoNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;

    public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();
}
