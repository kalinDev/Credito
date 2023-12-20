using System;
using System.Linq;
using LiberacaoCredito.Application.Validations;
using LiberacaoCredito.Domain.Enums;
using LiberacaoCredito.Domain.Interfaces;
using LiberacaoCredito.Domain.Models;

namespace LiberacaoCredito.Application.Services;

public class CreditoService : ICreditoService
{
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

    private decimal ObterTaxaJuros(TipoCredito tipoCredito)
    {
        switch (tipoCredito)
        {
            case TipoCredito.CreditoDireto:
                return 2;
            case TipoCredito.CreditoConsignado:
                return 1;
            case TipoCredito.CreditoPessoaJuridica:
                return 5;
            case TipoCredito.CreditoPessoaFisica:
                return 3;
            case TipoCredito.CreditoImobiliario:
                return 9;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}