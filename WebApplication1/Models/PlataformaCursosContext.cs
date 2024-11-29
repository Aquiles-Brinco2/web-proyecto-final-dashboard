using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models;

public partial class PlataformaCursosContext : DbContext
{
    public PlataformaCursosContext()
    {
    }

    public PlataformaCursosContext(DbContextOptions<PlataformaCursosContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Calificacione> Calificaciones { get; set; }

    public virtual DbSet<Categoria> Categorias { get; set; }

    public virtual DbSet<Comentario> Comentarios { get; set; }

    public virtual DbSet<Curso> Cursos { get; set; }

    public virtual DbSet<CursoCategorium> CursoCategoria { get; set; }

    public virtual DbSet<Factura> Facturas { get; set; }

    public virtual DbSet<Inscripcione> Inscripciones { get; set; }

    public virtual DbSet<Leccione> Lecciones { get; set; }

    public virtual DbSet<Materiale> Materiales { get; set; }

    public virtual DbSet<Pago> Pagos { get; set; }

    public virtual DbSet<Soporte> Soportes { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    { }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Calificacione>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Califica__3214EC07469F71A7");

            entity.Property(e => e.FechaCalificacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.IdCursoNavigation).WithMany(p => p.Calificaciones)
                .HasForeignKey(d => d.IdCurso)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Calificac__IdCur__29572725");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Calificaciones)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Calificac__IdUsu__2A4B4B5E");
        });

        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Categori__3214EC07FF55C8F9");

            entity.HasIndex(e => e.Nombre, "UQ__Categori__75E3EFCFACD9408E").IsUnique();

            entity.Property(e => e.Nombre).HasMaxLength(100);
        });

        modelBuilder.Entity<Comentario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Comentar__3214EC0731E820BB");

            entity.Property(e => e.FechaComentario)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.IdCursoNavigation).WithMany(p => p.Comentarios)
                .HasForeignKey(d => d.IdCurso)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Comentari__IdCur__239E4DCF");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Comentarios)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Comentari__IdUsu__24927208");
        });

        modelBuilder.Entity<Curso>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Cursos__3214EC076578B584");

            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Titulo).HasMaxLength(255);

            entity.HasOne(d => d.IdInstructorNavigation).WithMany(p => p.Cursos)
                .HasForeignKey(d => d.IdInstructor)
                .HasConstraintName("FK__Cursos__IdInstru__164452B1");
        });

        modelBuilder.Entity<CursoCategorium>(entity =>
        {
            entity.HasKey(e => new { e.IdCurso, e.IdCategoria }).HasName("PK__Curso_Ca__12632577C7D77A92");

            entity.ToTable("Curso_Categoria");

            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.IdCategoriaNavigation).WithMany(p => p.CursoCategoria)
                .HasForeignKey(d => d.IdCategoria)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Curso_Cat__IdCat__35BCFE0A");

            entity.HasOne(d => d.IdCursoNavigation).WithMany(p => p.CursoCategoria)
                .HasForeignKey(d => d.IdCurso)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Curso_Cat__IdCur__34C8D9D1");
        });

        modelBuilder.Entity<Factura>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Facturas__3214EC074261CABB");

            entity.HasIndex(e => e.NumeroFactura, "UQ__Facturas__CF12F9A6D5170524").IsUnique();

            entity.Property(e => e.FechaFactura)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Monto).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.NumeroFactura).HasMaxLength(50);

            entity.HasOne(d => d.IdPagoNavigation).WithMany(p => p.Facturas)
                .HasForeignKey(d => d.IdPago)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Facturas__IdPago__46E78A0C");
        });

        modelBuilder.Entity<Inscripcione>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Inscripc__3214EC07F56D9880");

            entity.Property(e => e.Estado)
                .HasMaxLength(20)
                .HasDefaultValue("activo");
            entity.Property(e => e.FechaInscripcion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.IdCursoNavigation).WithMany(p => p.Inscripciones)
                .HasForeignKey(d => d.IdCurso)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Inscripci__IdCur__1FCDBCEB");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Inscripciones)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Inscripci__IdUsu__1ED998B2");
        });

        modelBuilder.Entity<Leccione>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Leccione__3214EC071CAD7F92");

            entity.Property(e => e.Titulo).HasMaxLength(255);

            entity.HasOne(d => d.IdCursoNavigation).WithMany(p => p.Lecciones)
                .HasForeignKey(d => d.IdCurso)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Lecciones__IdCur__1920BF5C");
        });

        modelBuilder.Entity<Materiale>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Material__3214EC077CFE5933");

            entity.Property(e => e.Tipo).HasMaxLength(20);
            entity.Property(e => e.Url).HasMaxLength(255);

            entity.HasOne(d => d.IdCursoNavigation).WithMany(p => p.Materiales)
                .HasForeignKey(d => d.IdCurso)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Materiale__IdCur__2E1BDC42");
        });

        modelBuilder.Entity<Pago>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Pagos__3214EC077C7F31D2");

            entity.Property(e => e.Estado)
                .HasMaxLength(20)
                .HasDefaultValue("completado");
            entity.Property(e => e.FechaPago)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.MetodoPago).HasMaxLength(50);
            entity.Property(e => e.Monto).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.IdInscripcionNavigation).WithMany(p => p.Pagos)
                .HasForeignKey(d => d.IdInscripcion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Pagos__IdInscrip__4222D4EF");
        });

        modelBuilder.Entity<Soporte>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Soporte__3214EC074788804E");

            entity.ToTable("Soporte");

            entity.Property(e => e.Estado)
                .HasMaxLength(20)
                .HasDefaultValue("pendiente");
            entity.Property(e => e.FechaMensaje)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Soportes)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Soporte__IdUsuar__3B75D760");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Usuarios__3214EC0741350D19");

            entity.HasIndex(e => e.Email, "UQ__Usuarios__A9D10534A313D2A3").IsUnique();

            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.Tipo).HasMaxLength(20);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
