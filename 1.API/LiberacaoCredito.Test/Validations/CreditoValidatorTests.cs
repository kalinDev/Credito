using FluentValidation.TestHelper;
using LiberacaoCredito.Application.Validations;
using LiberacaoCredito.Domain.Enums;
using LiberacaoCredito.Domain.Models;

namespace LiberacaoCredito.Test.Validations;

public class CreditoValidatorTests
{
    #region ValorCredito Tests
    
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-1000)]
    [Trait("ValidacaoCredito", "ValorCredito")]
    public void ValorCredito_ShouldHaveValidationErrorWhenValueIsZeroOrNegative(decimal valor)
    {
        var validator = new CreditoValidator();
        var credito = new Credito { ValorCredito = valor };

        var result = validator.TestValidate(credito);

        result.ShouldHaveValidationErrorFor(c => c.ValorCredito)
            .WithErrorMessage(CreditoValidator.ValorCreditoMaiorQueZeroMessage);
    }
    
    [Theory]
    [InlineData(0.1)]
    [InlineData(100)]
    [InlineData(20000)]
    [Trait("ValidacaoCredito", "ValorCredito")]
    public void ValorCredito_ShouldNotHaveValidationErrorWhenValueIsGreaterThanZero(decimal valor)
    {
        var validator = new CreditoValidator();
        var credito = new Credito { ValorCredito = valor };

        var result = validator.TestValidate(credito);

        result.ShouldNotHaveValidationErrorFor(c => c.ValorCredito);
    }
    #endregion
    
    #region QuantidadeParcelas Tests

    [Theory]
    [InlineData(5)]
    [InlineData(10)]
    [InlineData(72)]
    [Trait("ValidacaoCredito", "QuantidadeParcelas")]
    public void QuantidadeParcelas_ShouldNotHaveValidationErrorWhenWithinRange(int quantidadeParcelas)
    {
        var validator = new CreditoValidator();
        var credito = new Credito { QuantidadeParcelas = quantidadeParcelas };

        var result = validator.TestValidate(credito);

        result.ShouldNotHaveValidationErrorFor(c => c.QuantidadeParcelas);
    }
    
    [Theory]
    [InlineData(-10)]
    [InlineData(4)]
    [InlineData(73)]
    [InlineData(200)]
    [Trait("ValidacaoCredito", "QuantidadeParcelas")]
    public void QuantidadeParcelas_ShouldHaveValidationErrorWhenOutOfRange(int quantidadeParcelas)
    {
        var validator = new CreditoValidator();
        var credito = new Credito { QuantidadeParcelas = quantidadeParcelas };

        var result = validator.TestValidate(credito);

        result.ShouldHaveValidationErrorFor(c => c.QuantidadeParcelas)
            .WithErrorMessage(CreditoValidator.QuantidadeParcelasOutOfRangeMessage);
    }

    #endregion
    
    #region DataPrimeiroVencimento Tests

    [Theory]
    [InlineData(15)]
    [InlineData(25)]
    [InlineData(40)]
    [Trait("ValidacaoCredito", "DataPrimeiroVencimento")]
    public void DataPrimeiroVencimento_ShouldNotHaveValidationErrorWhenWithinRange(int diasParaSomar)
    {
        var validator = new CreditoValidator();
        var credito = new Credito { DataPrimeiroVencimento = DateTime.Today.AddDays(diasParaSomar) };

        var result = validator.TestValidate(credito);

        result.ShouldNotHaveValidationErrorFor(c => c.DataPrimeiroVencimento);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(14)]
    [InlineData(41)]
    [InlineData(200)]
    [Trait("ValidacaoCredito", "DataPrimeiroVencimento")]
    public void DataPrimeiroVencimento_ShouldHaveValidationErrorWhenOutOfRange(int diasParaSomar)
    {
        var validator = new CreditoValidator();
        var credito = new Credito { DataPrimeiroVencimento = DateTime.Today.AddDays(diasParaSomar) };

        var result = validator.TestValidate(credito);

        result.ShouldHaveValidationErrorFor(c => c.DataPrimeiroVencimento)
            .WithErrorMessage(CreditoValidator.DataPrimeiroVencimentoOutOfRangeMessage);
    }
    
    #endregion
    
    #region TipoCredito Tests

    [Fact]
    [Trait("ValidacaoCredito", "TipoCredito")]
    public void TipoCredito_ShouldNotHaveValidationErrorWhenValidEnumValue()
    {
        var validator = new CreditoValidator();
        var credito = new Credito { TipoCredito = TipoCredito.CreditoPessoaFisica, ValorCredito = 50000 };

        var result = validator.TestValidate(credito);

        result.ShouldNotHaveValidationErrorFor(c => c.TipoCredito);
    }

    [Fact]
    [Trait("ValidacaoCredito", "TipoCredito")]
    public void TipoCredito_ShouldHaveValidationErrorWhenInvalidEnumValue()
    {
        var validator = new CreditoValidator();
        var credito = new Credito { TipoCredito = (TipoCredito)100, ValorCredito = 50000 };

        var result = validator.TestValidate(credito);

        result.ShouldHaveValidationErrorFor(c => c.TipoCredito)
            .WithErrorMessage(CreditoValidator.TipoCreditoInvalidEnumMessage);
    }
    
    [Theory]
    [InlineData(15000)]
    [InlineData(15000.01)]
    [InlineData(1000000)]
    [Trait("ValidacaoCredito", "TipoCredito")]
    public void TipoCredito_ShouldNotHaveValidationErrorWhenValidEnumValueForCreditoPessoaJuridica(decimal valorCredito)
    {
        var validator = new CreditoValidator();
        var credito = new Credito { TipoCredito = TipoCredito.CreditoPessoaJuridica, ValorCredito = valorCredito };

        var result = validator.TestValidate(credito);

        result.ShouldNotHaveValidationErrorFor(c => c.TipoCredito);
    }
    
    [Theory]
    [InlineData(-15000)]
    [InlineData(1000)]
    [InlineData(14999.9)]
    [Trait("ValidacaoCredito", "TipoCredito")]
    public void TipoCredito_ShouldHaveValidationErrorWhenCreditoPessoaJuridicaAndValorCreditoLessThan15000(decimal valorCredito)
    {
        var validator = new CreditoValidator();
        var credito = new Credito { TipoCredito = TipoCredito.CreditoPessoaJuridica, ValorCredito = valorCredito };

        var result = validator.TestValidate(credito);

        result.ShouldHaveValidationErrorFor(c => c.TipoCredito)
            .WithErrorMessage(CreditoValidator.TipoCreditoInvalidForRequestedAmountMessage);
    }

    #endregion
}