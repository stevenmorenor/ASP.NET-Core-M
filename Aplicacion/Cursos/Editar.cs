using System;
using System.Net;
using Aplicacion.ManejadorError;
using FluentValidation;
using MediatR;
using Persistencia;



namespace Aplicacion.Cursos
{
    public class Editar
    {
        public class Ejecuta : IRequest
        {
            public int CursoId { get; set; }
            public string? Titulo { get; set; }
            public string? Descripcion { get; set; }
            public DateTime? FechaPublicacion { get; set; }

        }
    }
    public class EjecutaValicadion : AbstractValidator<Editar.Ejecuta>
        {
            public EjecutaValicadion()
            {
                RuleFor( x => x.Titulo).NotEmpty();
                RuleFor( x => x.Descripcion).NotEmpty();
                RuleFor( x => x.FechaPublicacion).NotEmpty();
            }
        }

    public class Manejador : IRequestHandler<Editar.Ejecuta>
    {
        private readonly CursosOnlineContext _context;
        public Manejador(CursosOnlineContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(Editar.Ejecuta request, CancellationToken cancellationToken)
        {
            var curso = await _context.Curso.FindAsync(request.CursoId);
           if(curso==null)
                {
                    //throw new Exception("No se puede eliminar curso");
                    throw new ManejadorExcepcion(HttpStatusCode.NotFound, new {Mensaje = "No se encontro el curso"});
                }

            curso.Titulo = request.Titulo ?? curso.Titulo;
            curso.Descripcion = request.Descripcion ?? curso.Descripcion;
            curso.FechaPublicacion = request.FechaPublicacion ?? curso.FechaPublicacion;

            var resultado = await _context.SaveChangesAsync();

            if(resultado>0)
            {
                return Unit.Value;
            }
            throw new Exception("No se guardaron los datos en el curso");
        }
    }

}