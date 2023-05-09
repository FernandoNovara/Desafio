using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Desafio.Controllers;

[ApiController]
[Route("[controller]")]
public class RegisterController : ControllerBase
{

    private readonly DataContext Contexto;

    public RegisterController(DataContext context)
    {
        this.Contexto = context;
    }

    [HttpGet]
    public async Task<ActionResult<Register>> Get()
    {
        try
        {
            var registro = await Contexto.register.ToListAsync() ;
            return Ok(registro);
        }
        catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

//      Generar los servicios correspondientes, los cuales nos permitan guardar en base
//      de datos, los ingresos y egresos del personal.
//      Function register(idEmployee, date, registerType, businessLocation)


    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromForm]Register registro)
    {
        try
        {
            if(registro.RegisterType.Equals("Ingreso") && registro.RegisterType != null)
            {
                registro.RegisterType = "Egreso";
                registro.Date = DateTime.Now;
            }
            else
            {
                registro.RegisterType = "Ingreso";
                registro.Date = DateTime.Now;
            }

            await Contexto.AddAsync(registro);
            Contexto.SaveChanges();
            return Ok(registro);
        
        }
        catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}