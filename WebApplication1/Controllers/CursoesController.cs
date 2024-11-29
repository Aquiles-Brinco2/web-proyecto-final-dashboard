using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class CursoesController : Controller
    {
        private readonly PlataformaCursosContext _context;

        public CursoesController(PlataformaCursosContext context)
        {
            _context = context;
        }
        // Controlador de Mis Cursos
        public async Task<IActionResult> MisCursos()
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var misCursos = await (from ins in _context.Inscripciones
                                   join curso in _context.Cursos on ins.IdCurso equals curso.Id
                                   join calif in _context.Calificaciones on new { ins.IdCurso, ins.IdUsuario } equals new { calif.IdCurso, calif.IdUsuario } into calificacionesGroup
                                   from calif in calificacionesGroup.DefaultIfEmpty() // Manejar si no hay calificación
                                   where ins.IdUsuario == int.Parse(userId)
                                   select new MisCursosViewModel
                                   {
                                       CursoId = curso.Id,
                                       TituloCurso = curso.Titulo,
                                       DescripcionCurso = curso.Descripcion,
                                       FechaInscripcion = ins.FechaInscripcion, // nullable DateTime?
                                       Calificacion = calif != null ? (int?)calif.Calificacion : null // nullable int?, manejar si no hay calificación
                                   }).ToListAsync();

            return View(misCursos);
        }

        public IActionResult Certificado(int cursoId)
        {
            var userId = HttpContext.Session.GetString("UserId");
            var alumnoNombre = HttpContext.Session.GetString("UserName"); // Suponiendo que almacenas el nombre en la sesión.

            // Obtener el título del curso
            var curso = _context.Cursos.Find(cursoId);

            if (curso == null)
            {
                return NotFound();
            }

            var modelo = new CertificadoViewModel
            {
                AlumnoNombre = alumnoNombre,
                TituloCurso = curso.Titulo
            };

            return View(modelo);
        }



        public IActionResult Lecciones(int cursoId)
        {
            // Traer todas las lecciones que corresponden al curso
            var lecciones = _context.Lecciones
                .Where(l => l.IdCurso == cursoId)
                .ToList();

            // Verificar si hay lecciones
            if (lecciones == null || !lecciones.Any())
            {
                return NotFound("No hay lecciones disponibles para este curso.");
            }

            return View(lecciones);  // Pasamos la lista de lecciones a la vista
        }

        // Método para mostrar los materiales de un curso
        public IActionResult Materiales(int cursoId)
        {
            // Traer todos los materiales que corresponden al curso
            var materiales = _context.Materiales
                .Where(m => m.IdCurso == cursoId)
                .ToList();

            // Verificar si hay materiales
            if (materiales == null || !materiales.Any())
            {
                return NotFound("No hay materiales disponibles para este curso.");
            }

            return View(materiales);  // Pasamos la lista de materiales a la vista
        }







        public IActionResult EditarNota(int cursoId, int usuarioId)
        {
            var calificacione = _context.Calificaciones
                .FirstOrDefault(c => c.IdCurso == cursoId && c.IdUsuario == usuarioId);

            if (calificacione == null)
            {
                // Manejar el caso donde no existe una calificación
                return NotFound();
            }

            return View(calificacione);
        }
        


        [HttpPost]
        public IActionResult EditarNota(Calificacione calificacione)
        {
            if (ModelState.IsValid)
            {
                _context.Update(calificacione);
                _context.SaveChanges();
                return RedirectToAction(nameof(MisCursosInstructor));
            }

            return View(calificacione);
        }


        public IActionResult MisCursosInstructor()
        {
            // Obtener el instructor actual (usando la sesión o el usuario autenticado)
            var instructorId = int.Parse(HttpContext.Session.GetString("UserId"));

            // Obtener los cursos que enseña el instructor
            var cursos = _context.Cursos
                .Where(c => c.IdInstructor == instructorId)
                .Include(c => c.Inscripciones) // Obtener las inscripciones de los cursos
                    .ThenInclude(i => i.IdUsuarioNavigation) // Obtener los usuarios inscritos
                .Include(c => c.Calificaciones) // Obtener las calificaciones del curso
                .ToList();

            return View(cursos);
        }


        // GET: Cursoes
        public IActionResult Index(string searchString)
        {
            var cursos = from c in _context.Cursos
                         select c;

            if (!String.IsNullOrEmpty(searchString))
            {
                cursos = cursos.Where(c => c.Titulo.Contains(searchString));
            }

            return View(cursos.ToList());
        }


        // GET: Cursoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var curso = await _context.Cursos
                .Include(c => c.IdInstructorNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (curso == null)
            {
                return NotFound();
            }

            return View(curso);
        }

        // GET: Cursoes/Create
        public IActionResult Create()
        {
            // Filtrar los usuarios cuyo tipo sea "instructor"
            ViewData["IdInstructor"] = new SelectList(
                _context.Usuarios.Where(u => u.Tipo == "instructor"),
                "Id",
                "Nombre"
            );
            return View();
        }

        // POST: Cursoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Titulo,Descripcion,FechaCreacion,IdInstructor,Imagen")] Curso curso, IFormFile Imagen)
        {
            if (ModelState.IsValid)
            {
                // Verificar si se ha subido una imagen
                if (Imagen != null && Imagen.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        // Leer el archivo de imagen en el MemoryStream
                        await Imagen.CopyToAsync(memoryStream);

                        // Asignar el arreglo de bytes de la imagen al campo Imagen del modelo
                        curso.Imagen = memoryStream.ToArray();
                    }
                }

                _context.Add(curso);
                await _context.SaveChangesAsync();

                return RedirectToAction("Create", "Lecciones", new { idCurso = curso.Id });
            }


            return View(curso);
        }



        // GET: Cursoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var curso = await _context.Cursos.FindAsync(id);
            if (curso == null)
            {
                return NotFound();
            }
            ViewData["IdInstructor"] = new SelectList(_context.Usuarios, "Id", "Id", curso.IdInstructor);
            return View(curso);
        }

        // POST: Cursoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Titulo,Descripcion,FechaCreacion,IdInstructor,Imagen")] Curso curso, IFormFile Imagen)
        {
            if (id != curso.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Verificar si se ha subido una nueva imagen
                    if (Imagen != null && Imagen.Length > 0)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await Imagen.CopyToAsync(memoryStream);
                            curso.Imagen = memoryStream.ToArray();
                        }
                    }

                    // Si no se subió una nueva imagen, mantendrá la imagen que ya tiene el curso.
                    _context.Update(curso);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CursoExists(curso.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(curso);
        }

        // GET: Cursoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var curso = await _context.Cursos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (curso == null)
            {
                return NotFound();
            }

            return View(curso);
        }

        // POST: Cursoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var curso = await _context.Cursos.FindAsync(id);
            if (curso != null)
            {
                _context.Cursos.Remove(curso);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CursoExists(int id)
        {
            return _context.Cursos.Any(e => e.Id == id);
        }


    }
}
