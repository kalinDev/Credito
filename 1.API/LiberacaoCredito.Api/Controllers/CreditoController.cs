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
    
    /// <summary>
    /// Processa uma solicitação de crédito, realizando validações e retornando o resultado.
    /// </summary>
    /// <remarks>
    /// Este endpoint é acessível por meio de uma solicitação HTTP POST para a rota "Processar".
    /// O corpo da solicitação deve conter os detalhes da solicitação de crédito no formato especificado pela classe Credito.
    /// </remarks>
    /// <param name="credito">As informações da solicitação de crédito.</param>
    /// <returns>
    /// Um ActionResult contendo um objeto ResultadoCredito se a solicitação for bem-sucedida.
    /// Se a solicitação não for aprovada, retorna um BadRequest com detalhes sobre a não aprovação.
    /// Se ocorrer um erro interno durante o processamento, retorna um StatusCode 500 com uma mensagem de erro.
    /// </returns>
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