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

    [HttpGet("Average")]
    public async Task<IActionResult> Average([FromForm] String dateFrom,[FromForm] String dateTo)
    {
        try
        {
            // Obtengo las sucursales en la base
            var sucursales = Contexto.business.ToList();

            var registrosPorSucursales = Contexto.register
                    .GroupBy(r => new { r.business.Location})
                    .Select(g => new
                    {
                        Sucursal = g.Key.Location,
                        Cantidad = g.Count()
                    })
                    .ToList();

            // Obtengo todos los registros de la base de datos en el tiempo 
            var registros = Contexto.register
                    .Where(r => r.Date >= DateTime.Parse(dateFrom) && r.Date <= DateTime.Parse(dateTo))
                    .GroupBy(r => new { r.business.Location, r.employee.Genero}) // Agrupar por ubicación de la sucursal
                    .Select(g => new
                    {
                        Sucursal = g.Key.Location,
                        Genero = g.Key.Genero,
                        Cantidad = g.Count()// Contar el número de registros en cada grupo
                    })
                    .ToList();

            var promedios = sucursales.Select(s => new
            {
                Sucursal = s.Location,
                PromedioMasculino = ((decimal)registros.FirstOrDefault(r => r.Genero == "Masculino").Cantidad / registrosPorSucursales.FirstOrDefault(r => r.Sucursal == s.Location).Cantidad).ToString("0.00"),
                PromedioFemenino = ((decimal)registros.FirstOrDefault(r => r.Genero == "Femenino").Cantidad / registrosPorSucursales.FirstOrDefault(r => r.Sucursal == s.Location).Cantidad).ToString("0.00")
            }).ToList();

        return Ok(promedios);
        }
        catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}