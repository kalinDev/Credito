using FluentValidation;
using LiberacaoCredito.Domain.Enums;
using LiberacaoCredito.Domain.Models;

namespace LiberacaoCredito.Application.Validations;

public class CreditoValidator : AbstractValidator<Credito>
{
    public const string ValorCreditoMaiorQueZeroMessage = "O valor do crédito deve ser maior que zero.";
    public const string ValorCreditoMenorOuIgualUmMilhaoMessage = "O valor do crédito não pode ser maior que R$ 1.000.000,00.";
    public const string QuantidadeParcelasOutOfRangeMessage = "A quantidade de parcelas deve estar entre 5 e 72.";
    public const string DataPrimeiroVencimentoOutOfRangeMessage = "A data do primeiro vencimento deve estar entre 15 e 40 dias a partir da data atual.";
    public const string TipoCreditoInvalidEnumMessage = "Tipo de crédito inválido.";
    public const string TipoCreditoInvalidForRequestedAmountMessage = "Tipo de crédito inválido para o valor solicitado.";

    public CreditoValidator()
    {
        RuleFor(credito => credito.ValorCredito)
            .GreaterThan(0).WithMessage(ValorCreditoMaiorQueZeroMessage)
            .LessThanOrEqualTo(1000000).WithMessage(ValorCreditoMenorOuIgualUmMilhaoMessage);

        RuleFor(credito => credito.QuantidadeParcelas)
            .InclusiveBetween(5, 72).WithMessage(QuantidadeParcelasOutOfRangeMessage);

        RuleFor(credito => credito.DataPrimeiroVencimento)
            .Must(BeValidDate).WithMessage(DataPrimeiroVencimentoOutOfRangeMessage);

        RuleFor(credito => credito.TipoCredito)
            .IsInEnum().WithMessage(TipoCreditoInvalidEnumMessage)
            .Must((credito, tipoCredito) => TipoCreditoValido(credito, tipoCredito))
            .WithMessage(TipoCreditoInvalidForRequestedAmountMessage);

        RuleFor(credito => credito.TipoCredito)
            .IsInEnum().WithMessage(TipoCreditoInvalidEnumMessage);
    }

    private bool BeValidDate(DateTime date)
    {
        var dataMinimaVencimento = DateTime.Today.AddDays(15);
        var dataMaximaVencimento = DateTime.Today.AddDays(40);
        return date >= dataMinimaVencimento && date <= dataMaximaVencimento;
    }

    private bool TipoCreditoValido(Credito credito, TipoCredito tipoCredito)
    {
        return tipoCredito != TipoCredito.CreditoPessoaJuridica || credito.ValorCredito >= 15000;
    }
}