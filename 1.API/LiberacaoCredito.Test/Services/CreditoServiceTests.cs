using System;
using LiberacaoCredito.Application.Services;
using LiberacaoCredito.Domain.Enums;
using LiberacaoCredito.Domain.Models;

namespace LiberacaoCredito.Test.Services;

public class CreditoServiceTests
{
    [Fact]
    public void ProcessarCredito_ShouldReturnRejectedResultWhenCreditValidationFails()
    {
        // Arrange
        var creditoService = new CreditoService();
        var credito = new Credito();

        // Act
        var resultado = creditoService.ProcessarCredito(credito);

        // Assert
        Assert.False(resultado.Aprovado);
    }

    [Fact]
    public void ProcessarCredito_ShouldReturnApprovedResultWhenCreditValidationPasses()
    {
        // Arrange
        var creditoService = new CreditoService();

        var credito = new Credito
        {
            TipoCredito = TipoCredito.CreditoPessoaFisica,
            ValorCredito = 15000,
            QuantidadeParcelas = 5,
            DataPrimeiroVencimento = DateTime.Now.AddDays(20)
        };

        // Act
        var resultado = creditoService.ProcessarCredito(credito);

        // Assert
        Assert.True(resultado.Aprovado);
        Assert.True(resultado.ValorJuros > 0);
        Assert.True(resultado.ValorTotalComJuros > credito.ValorCredito);
    }

    [Fact]
    public void ProcessarCredito_ShouldReturnRejectedResultWhenTipoCreditoIsInvalid()
    {
        // Arrange
        var creditoService = new CreditoService();

        var credito = new Credito
        {
            TipoCredito = (TipoCredito)99, // Invalid credit type
            ValorCredito = 15000,
            QuantidadeParcelas = 5,
            DataPrimeiroVencimento = DateTime.Now.AddDays(20)
        };

        // Act
        var resultado = creditoService.ProcessarCredito(credito);

        // Assert
        Assert.False(resultado.Aprovado);
    }
    
    [Theory]
    [InlineData(TipoCredito.CreditoConsignado, 1)]
    [InlineData(TipoCredito.CreditoDireto, 2)]
    [InlineData(TipoCredito.CreditoPessoaFisica,3)]
    [InlineData(TipoCredito.CreditoPessoaJuridica,5)]
    [InlineData(TipoCredito.CreditoImobiliario,9)]
    public void ProcessarCredito_ShouldCalculateInterestForCreditType(TipoCredito tipoCredito, int juros)
    {
        // Arrange
        const decimal valorCredito = 15000;
        
        var valorJuros = juros * valorCredito / 100;
        var valorTotalComJuros = valorCredito + valorJuros;
        
        var credito = new Credito
        {
            TipoCredito = tipoCredito,
            ValorCredito = valorCredito,
            QuantidadeParcelas = 5,
            DataPrimeiroVencimento = DateTime.Now.AddDays(20)
        };
        
        var creditoService = new CreditoService();
        
        // Act
        var resultado = creditoService.ProcessarCredito(credito);

        // Assert
        Assert.True(resultado.Aprovado);
        Assert.Equal(valorJuros, resultado.ValorJuros); 
        Assert.Equal(valorTotalComJuros, resultado.ValorTotalComJuros); 
    }
    
}