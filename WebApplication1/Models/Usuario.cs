using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Usuario
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Tipo { get; set; } = null!;

    public DateTime? FechaRegistro { get; set; }

    public byte[]? Imagen { get; set; }

    public virtual ICollection<Calificacione> Calificaciones { get; set; } = new List<Calificacione>();

    public virtual ICollection<Comentario> Comentarios { get; set; } = new List<Comentario>();

    public virtual ICollection<Curso> Cursos { get; set; } = new List<Curso>();

    public virtual ICollection<Inscripcione> Inscripciones { get; set; } = new List<Inscripcione>();

    public virtual ICollection<Soporte> Soportes { get; set; } = new List<Soporte>();
}
