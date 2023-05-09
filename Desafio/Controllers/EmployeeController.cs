using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Desafio.Controllers;

[ApiController]
[Route("[controller]")]
public class EmployeeController : ControllerBase
{

    private readonly DataContext Contexto;

    public EmployeeController(DataContext context)
    {
        this.Contexto = context;
    }

    [HttpGet]
    public async Task<ActionResult<Employee>> Get()
    {
        try
        {
            var personal = await Contexto.employee.ToListAsync() ;
            return Ok(personal);
        }
        catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("AltaEmployee")]
    public async Task<IActionResult> AltaEmployee([FromForm]Employee personal)
    {
        try
        {
            await Contexto.AddAsync(personal);
            Contexto.SaveChanges();
            return CreatedAtAction(nameof(Get),new {id = personal.idEmployee},personal);
        }
        catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}