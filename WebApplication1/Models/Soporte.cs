using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Soporte
{
    public int Id { get; set; }

    public int IdUsuario { get; set; }

    public string Mensaje { get; set; } = null!;

    public DateTime? FechaMensaje { get; set; }

    public string? Estado { get; set; }

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
