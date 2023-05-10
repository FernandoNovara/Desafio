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


    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromForm] Register registro)
    {
        try
        {
            if (registro != null)
            {
                var res = await Contexto.register.OrderByDescending(x => x.Date).FirstOrDefaultAsync(x => x.IdEmployee == registro.IdEmployee);
                if (res == null)
                {
                    if (registro.RegisterType.Equals("Ingreso"))
                    {
                        registro.Date = DateTime.Now;
                        await Contexto.AddAsync(registro);
                        await Contexto.SaveChangesAsync();
                        
                        return Ok(registro);
                    }
                    else
                    {
                        return BadRequest("El tipo de registro no puede puede ser egreso.");
                    }
                }
                if (!registro.RegisterType.Equals(res.RegisterType))
                {
                    registro.Date = DateTime.Now;
                    await Contexto.AddAsync(registro);
                    await Contexto.SaveChangesAsync();

                    return Ok(registro);
                }
                else
                {
                    if (registro.RegisterType.Equals("Ingreso"))
                    {
                        return BadRequest("No marco un egreso");
                    }
                    else
                    {
                        return BadRequest("No marco un Ingreso");
                    }
                }
            }
            else
            {
                return BadRequest("No se puede cargar una registro nulo.");
            }
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

//      Generar un servicio el cual liste la cantidad de ingresos y egresos dada una fecha
//      desde – hasta, que se pueda filtrar por nombre o apellido y sucursal.
//      Function search(dateFrom, dateTo , descriptionFilter,
//      businessLocation)

    [HttpGet("Search")]
    public async Task<ActionResult<Register>> Search ([FromForm] String dateFrom,[FromForm] String dateTo,[FromForm] String? descriptionFilter,[FromForm] String? businessLocation)
    {
        List<Register> res;
        if(!String.IsNullOrEmpty(descriptionFilter))
        {
            if(!String.IsNullOrEmpty(businessLocation))
            {
                res = await Contexto.register
                .Include(e => e.employee)
                .Include(b => b.business)
                .Where(x => x.Date >= DateTime.Parse(dateFrom) && x.Date <= DateTime.Parse(dateTo) 
                        && (x.employee.Nombre.Contains(descriptionFilter) || x.employee.Apellido.Contains(descriptionFilter)) 
                        && (x.business.Location.Equals(businessLocation)))
                .ToListAsync();
            }
            else{
                res = await Contexto.register
                .Include(e => e.employee)
                .Include(b => b.business)
                .Where(x => x.Date >= DateTime.Parse(dateFrom) && x.Date <= DateTime.Parse(dateTo) 
                        && (x.employee.Nombre.Contains(descriptionFilter) || x.employee.Apellido.Contains(descriptionFilter)))
                .ToListAsync();
            }
        }
        else{
                res = await Contexto.register
                .Include(e => e.employee)
                .Include(b => b.business)
                .Where(x => x.Date >= DateTime.Parse(dateFrom) && x.Date <= DateTime.Parse(dateTo))
                .ToListAsync();
        }

        return Ok(res);
    }
}