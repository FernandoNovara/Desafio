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

    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromForm] Register registro)
    {
        try
        {
            if (registro != null)//reviso que el parametro que me paso no este vacio
            {
                //Obtengo el ultimo registro del personal
                var res = await Contexto.register.OrderByDescending(x => x.Date).FirstOrDefaultAsync(x => x.IdEmployee == registro.IdEmployee);
                //Reviso si tiene un registro, si no tiene paso a crearle uno
                if (res == null)
                {
                    //reviso que el registro sea de Ingreso
                    if (registro.RegisterType.Equals("Ingreso"))
                    {
                        registro.Date = DateTime.Now;
                        await Contexto.AddAsync(registro);
                        await Contexto.SaveChangesAsync();
                        
                        return Ok(registro);
                    }
                    else
                    {
                        // de lo contrario marca un mensaje de error
                        return BadRequest("El tipo de registro no puede puede ser egreso.");
                    }
                }
                //Reviso que el registerType no sea iguales
                if (!registro.RegisterType.Equals(res.RegisterType))
                {
                    //Agrego la fecha y hora del sistema
                    registro.Date = DateTime.Now;

                    //Agrego a registro la informacion de business y employee
                    registro.business = res.business;
                    registro.employee = res.employee;

                    //Guardo en la base de datos
                    await Contexto.AddAsync(registro);
                    await Contexto.SaveChangesAsync();

                    return Ok(registro);
                }
                else
                {
                    //Reviso que tipo de ingreso fue el que ocaciono el problema y envio un mensaje acorde
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
                //muestro un mensaje de error si me trata de enviar un mensaje de error
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
        //reviso si descriptionFilter es nulo
        if(!String.IsNullOrEmpty(descriptionFilter))
        {
            //reviso si businessLocation es nulo
            if(!String.IsNullOrEmpty(businessLocation))
            {
                //Si no es nulo, entonces procedo a consultar con los datos de filtrado
                res = await Contexto.register
                .Include(e => e.employee)
                .Include(b => b.business)
                .Where(x => x.Date >= DateTime.Parse(dateFrom) && x.Date <= DateTime.Parse(dateTo) 
                        && (x.employee.Nombre.Contains(descriptionFilter) || x.employee.Apellido.Contains(descriptionFilter)) 
                        && (x.business.Location.Equals(businessLocation)))
                .ToListAsync();
            }
            //si businessLocation es nulo, entonces hago el filtrado solo por el descriptionFilter
            else{
                res = await Contexto.register
                .Include(e => e.employee)
                .Include(b => b.business)
                .Where(x => x.Date >= DateTime.Parse(dateFrom) && x.Date <= DateTime.Parse(dateTo) 
                        && (x.employee.Nombre.Contains(descriptionFilter) || x.employee.Apellido.Contains(descriptionFilter)))
                .ToListAsync();
            }
        }
        //si no tengo filto, hago una consulta general
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

            // Obtengo todos los registros de la base de datos por empresas
            var registrosPorSucursales = Contexto.register
                    .GroupBy(r => new { r.business.Location})
                    .Select(g => new
                    {
                        Sucursal = g.Key.Location,
                        Cantidad = g.Count()
                    })
                    .ToList();

            // Obtengo todos los registros de la base de datos en el tiempo,sucursal y genero
            var registros = Contexto.register
                    .Where(r => r.Date >= DateTime.Parse(dateFrom) && r.Date <= DateTime.Parse(dateTo))
                    .GroupBy(r => new { r.business.Location, r.employee.Genero}) // Agrupar por ubicación de la sucursal y genero
                    .Select(g => new
                    {
                        Sucursal = g.Key.Location,
                        Genero = g.Key.Genero,
                        Cantidad = g.Count()// Contar el número de registros en cada grupo
                    })
                    .ToList();

            //Obtengo el promedio haciendo una consulta seleccionando la sucursal y obteniendo atraves de una operacion matetica el promedio
            var promedios = sucursales.Select(s => new
            {
                Sucursal = s.Location,
                Promedio_Masculino = ((decimal)registros.FirstOrDefault(r => r.Genero == "Masculino").Cantidad / registrosPorSucursales.FirstOrDefault(r => r.Sucursal == s.Location).Cantidad).ToString("0.00"),
                Promedio_Femenino = ((decimal)registros.FirstOrDefault(r => r.Genero == "Femenino").Cantidad / registrosPorSucursales.FirstOrDefault(r => r.Sucursal == s.Location).Cantidad).ToString("0.00")
            }).ToList();

        return Ok(promedios);
        }
        catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}