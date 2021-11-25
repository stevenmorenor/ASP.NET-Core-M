using Aplicacion.Seguridad;
using Dominio;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [AllowAnonymous]
    public class UsuarioController : MiControllerBase
    {
        //Http//localhost:5100/api/Usuario/login
        [HttpPost("login")]
        public async Task<ActionResult<UsuarioData>> Login(Login.Ejecuta parametros)
        {
            return await Mediator.Send(parametros);
        }

        //Http//localhost:5100/api/Usuario/registrar
        [HttpPost("registrar")]
        public async Task<ActionResult<UsuarioData>> Registrar(Registrar.Ejecuta parametros)
        {
            return await Mediator.Send(parametros);
        }

        [HttpGet]
        //http://localhost:5100/api/Usuario
        public async Task<ActionResult<UsuarioData>> DevolverUsuario()
        {
            return await Mediator.Send(new UsuarioActual.Ejecutar());
        }

    }
}