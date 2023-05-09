using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Desafio.Controllers;

[ApiController]
[Route("[controller]")]
public class BusinessController : ControllerBase
{

    private readonly DataContext Contexto;

    public BusinessController(DataContext context)
    {
        this.Contexto = context;
    }

    [HttpGet]
    public async Task<ActionResult<Business>> Get()
    {
        try
        {
            var empresas = await Contexto.business.ToListAsync() ;
            return Ok(empresas);
        }
        catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("AltaBusiness")]
    public async Task<IActionResult> AltaBusiness([FromForm]Business empresas)
    {
        try
        {
            await Contexto.AddAsync(empresas);
            Contexto.SaveChanges();
            return CreatedAtAction(nameof(Get),new {id = empresas.IdBusiness},empresas);
        }
        catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}