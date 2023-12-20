using System;
using System.Linq;
using LiberacaoCredito.Application.Validations;
using LiberacaoCredito.Domain.Enums;
using LiberacaoCredito.Domain.Interfaces;
using LiberacaoCredito.Domain.Models;

namespace LiberacaoCredito.Application.Services;

public class CreditoService : ICreditoService
{
    /// <summary>
    /// Processa uma solicitação de crédito, realizando validações e calculando resultados.
    /// </summary>
    /// <param name="credito">As informações da solicitação de crédito.</param>
    /// <returns>Um objeto <see cref="ResultadoCredito"/> que contém informações sobre a aprovação,
    /// valor total com juros e valor dos juros, se a solicitação for aprovada.</returns>
    public ResultadoCredito ProcessarCredito(Credito credito)
    {
        // Validações
        var validator = new CreditoValidator();
        var validationResult = validator.Validate(credito);

        if (!validationResult.IsValid)
        {
            return new ResultadoCredito { Aprovado = false, MensagensErro = validationResult.Errors.Select(e => e.ErrorMessage).ToList() };
        }

        //Prorocessamento
        decimal taxaJuros = ObterTaxaJuros(credito.TipoCredito);
        decimal valorJuros = credito.ValorCredito * (taxaJuros / 100);
        decimal valorTotalComJuros = credito.ValorCredito + valorJuros;

        return new ResultadoCredito
        {
            Aprovado = true,
            ValorTotalComJuros = valorTotalComJuros,
            ValorJuros = valorJuros
        };
    }

    /// <summary>
    /// Obtém a taxa de crédito com base no tipo de crédito.
    /// Em um ambiente produtivo, as taxas seriam recuperadas de uma fonte externa.
    /// </summary>
    /// <param name="tipoCredito">O tipo de crédito.</param>
    /// <returns>A taxa correspondente ao tipo de crédito.</returns>
    private int ObterTaxaJuros(TipoCredito tipoCredito) =>
        tipoCredito switch
        {
            TipoCredito.CreditoDireto => 2,
            TipoCredito.CreditoConsignado => 1,
            TipoCredito.CreditoPessoaJuridica => 5,
            TipoCredito.CreditoPessoaFisica => 3,
            TipoCredito.CreditoImobiliario => 9,
            _ => throw new ArgumentOutOfRangeException()
        };

}