using Dominio;
using Microsoft.AspNetCore.Mvc;
using Persistencia;
using Microsoft.AspNetCore.Authentication;

namespace WebAPI.Controllers;

//   http://localhost:5100/WeatherForecast

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly CursosOnlineContext context;
    private readonly ISystemClock systemClock;

    public WeatherForecastController(CursosOnlineContext _context, ISystemClock systemClock)
    {
        this.context = _context;
        this.systemClock = systemClock;
    }

    [HttpGet]
    public IEnumerable<Curso> Get()
    {
        var result = context.Curso.ToList();
        return result;
    }
}
