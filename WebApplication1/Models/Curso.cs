using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Curso
{
    public int Id { get; set; }

    public string Titulo { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public DateTime? FechaCreacion { get; set; }

    public int? IdInstructor { get; set; }

    public byte[]? Imagen { get; set; }

    public virtual ICollection<Calificacione> Calificaciones { get; set; } = new List<Calificacione>();

    public virtual ICollection<Comentario> Comentarios { get; set; } = new List<Comentario>();

    public virtual ICollection<CursoCategorium> CursoCategoria { get; set; } = new List<CursoCategorium>();

    public virtual Usuario? IdInstructorNavigation { get; set; }

    public virtual ICollection<Inscripcione> Inscripciones { get; set; } = new List<Inscripcione>();

    public virtual ICollection<Leccione> Lecciones { get; set; } = new List<Leccione>();

    public virtual ICollection<Materiale> Materiales { get; set; } = new List<Materiale>();
}
