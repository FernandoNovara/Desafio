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
            return Ok("Hola Lucas Manco. No,no falla");
        }
        catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}