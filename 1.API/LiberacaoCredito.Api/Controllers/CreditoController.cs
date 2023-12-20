using System;
using LiberacaoCredito.Domain.Interfaces;
using LiberacaoCredito.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LiberacaoCredito.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class CreditoController : ControllerBase
{
    private readonly ICreditoService _service;
    private readonly ILogger<CreditoController> _logger;

    public CreditoController(ICreditoService service, ILogger<CreditoController> logger)
    {
        _service = service;
        _logger = logger;
    }
    
    [HttpPost("Processar")]
    public ActionResult<ResultadoCredito> ProcessarCredito([FromBody] Credito credito)
    {
        try
        {
            var resultado = _service.ProcessarCredito(credito);
            return resultado.Aprovado ? Ok(resultado) : BadRequest(resultado);
        }
        catch (Exception ex)
        {
            _logger.LogError("Erro ao processar pedido de crédito: {Erro}", ex.Message);
            return StatusCode(500, "Erro interno ao processar pedido de crédito.");
        }
    }
    
}