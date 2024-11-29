using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Categoria
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<CursoCategorium> CursoCategoria { get; set; } = new List<CursoCategorium>();
}
