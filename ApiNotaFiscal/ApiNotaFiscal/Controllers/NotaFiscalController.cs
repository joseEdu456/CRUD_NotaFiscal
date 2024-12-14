using ApiNotaFiscal.Data;
using ApiNotaFiscal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace ApiNotaFiscal.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NotaFiscalController : ControllerBase
{
    private readonly ApplicationContext _context;

    public NotaFiscalController(ApplicationContext context)
    {
        _context = context;
    }
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<IEnumerable<NotaFiscal>>> GetNotasFiscal()
    {
        var notas = await _context.NotaFiscal.AsNoTracking().ToListAsync();
        if (notas == null || notas.Count == 0)
        {
            return ValidationProblem(new ValidationProblemDetails(ModelState)
            {
                Title = "Nenhum nota fiscal cadastrada"
            });
        }
        return notas;
    }
    
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<NotaFiscal>> GetNotaFiscal_Id(int id)
    {
        var notas = await _context.NotaFiscal.ToListAsync();
        if (notas == null || notas.Count == 0)
        {
            return ValidationProblem(new ValidationProblemDetails(ModelState)
            {
                Title = "Nenhum nota fiscal cadastrada"
            });
        }

        if (!_context.NotaFiscal.Any(n => n.Id == id))
        {
            return ValidationProblem(new ValidationProblemDetails(ModelState)
            {
                Title = "Essa nota não foi cadastrada"
            });
        }
        
        var nota = await _context.NotaFiscal.FindAsync(id);
        
        return nota;
    }

    [HttpGet("{data:datetime}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<IEnumerable<NotaFiscal>>> GetNotaFiscalData(DateTime data)
    {
        var notas = await _context.NotaFiscal.ToListAsync();
        if (notas == null || notas.Count == 0)
        {
            return ValidationProblem(new ValidationProblemDetails(ModelState)
            {
                Title = "Nenhum nota fiscal cadastrada"
            });
        }
        
        var notasPorData = notas.Where(n => n.DataEmissao.ToShortDateString() == data.ToShortDateString()).ToList();
        if (notasPorData == null || notasPorData.Count == 0)
        {
            return ValidationProblem(new ValidationProblemDetails(ModelState)
            {
                Title = "Nenhum nota fiscal encontrada com essa data"
            });
        }
        return notasPorData;
    }

    [HttpGet("valor/{valor:decimal}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<IEnumerable<NotaFiscal>>> GetNotaFiscalValor(decimal valor)
    {
        var notas = await _context.NotaFiscal.ToListAsync();
        if (notas == null || notas.Count == 0)
        {
            return ValidationProblem(new ValidationProblemDetails(ModelState)
            {
                Title = "Nenhum nota fiscal cadastrada"
            });
        }
        
        var notasPorValor =  notas.Where(n => n.Valor == valor).ToList();
        if (notasPorValor == null || notasPorValor.Count == 0)
        {
            return ValidationProblem(new ValidationProblemDetails(ModelState)
            {
                Title = "Nenhum nota fiscal encontrada com esse valor"
            });
        }
        return notasPorValor;
    }
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<NotaFiscal>> PostNotaFiscal(NotaFiscal notaFiscal)
    {
        if(!ModelState.IsValid){return ValidationProblem(ModelState);}
        
        if (notaFiscal != null)
        {
            _context.NotaFiscal.Add(notaFiscal);
            await _context.SaveChangesAsync();
            
        }
        else
        {
            return BadRequest();
        }
        
        return CreatedAtAction(nameof(GetNotaFiscal_Id), new { id = notaFiscal.Id }, notaFiscal);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<NotaFiscal>> PutNotaFiscal(int id, NotaFiscal notaFiscal)
    {
        var notas = await _context.NotaFiscal.ToListAsync();
        if (notas == null || notas.Count == 0)
        {
            return ValidationProblem(new ValidationProblemDetails(ModelState)
            {
                Title = "Nenhum nota fiscal cadastrada"
            });
        }
        
        if(id != notaFiscal.Id) 
        {
            return ValidationProblem(new ValidationProblemDetails(ModelState)
            {
                Title = "Ids não são iguais"
            });
            
        }
        
        if (!ModelState.IsValid) {return BadRequest(ModelState);}
        
        var notaFiscalModified = await _context.NotaFiscal.FindAsync(id);
        if(notaFiscal.Valor != 0){notaFiscalModified.Valor = notaFiscal.Valor;}
        if(notaFiscal.Numero != 0){notaFiscalModified.Numero = notaFiscal.Numero;}

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!NotaFiscalExists(id))
            {
                return ValidationProblem(new ValidationProblemDetails(ModelState)
                {
                    Title = "Essa nota não foi cadastrada"
                });
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<NotaFiscal>> DeleteNotaFiscal(int id)
    {
        var notas = await _context.NotaFiscal.ToListAsync();
        if (notas == null || notas.Count == 0)
        {
            return ValidationProblem(new ValidationProblemDetails(ModelState)
            {
                Title = "Nenhum nota fiscal cadastrada"
            });
        }
        
        if (!_context.NotaFiscal.Any(n => n.Id == id))
        {
            return ValidationProblem(new ValidationProblemDetails(ModelState)
            {
                Title = "Essa nota não foi cadastrada"
            });
        }
        
        var notaFiscal = await _context.NotaFiscal.FindAsync(id);
        _context.NotaFiscal.Remove(notaFiscal);
        await _context.SaveChangesAsync();
        
        return NoContent();
    }

    private bool NotaFiscalExists(int id)
    {
        return _context.NotaFiscal.Any(e => e.Id == id);
    }
}