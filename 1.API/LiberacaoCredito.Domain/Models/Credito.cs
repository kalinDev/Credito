using LiberacaoCredito.Domain.Enums;

namespace LiberacaoCredito.Domain.Models;

public class Credito
{
    public decimal ValorCredito { get; set; }
    public TipoCredito TipoCredito { get; set; }
    public int QuantidadeParcelas { get; set; }
    public DateTime DataPrimeiroVencimento { get; set; }
}