using System.Collections.Generic;

namespace LiberacaoCredito.Domain.Models;

public class ResultadoCredito
{
    public bool Aprovado { get; set; }
    public decimal ValorTotalComJuros { get; set; }
    public decimal ValorJuros { get; set; }
    public List<string>? MensagensErro { get; set; } 
}