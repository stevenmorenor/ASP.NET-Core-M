using MediatR;
using Dominio;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Aplicacion.Cursos;
using Microsoft.AspNetCore.Authorization;

namespace WebAPI.Controllers
{
    //  http://localhost:5100/api/Cursos
    [Route("api/[controller]")]
    [ApiController]
    public class CursosController : MiControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<Curso>>> Get()
        {
            return await Mediator.Send(new Consulta.ListaCursos());
        }

        //  http://localhost:5100/api/Cursos/{id}
        [HttpGet("{id}")]

        public async Task<ActionResult<Curso>> Detalle(int id)
        {
            return await Mediator.Send(new ConsultaId.CursoUnico{Id = id});
        }
        
        [HttpPost]
        public async Task<ActionResult<Unit>> Crear(Nuevo.Ejecuta data)
        {
            return await Mediator.Send(data);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Unit>> Editar(int id,Editar.Ejecuta data)
        {
            data.CursoId = id;
            return await Mediator.Send(data);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> Eliminar (int id)
        {
            return await Mediator.Send(new Eliminar.Ejecuta{Id = id});
        }
    }
}