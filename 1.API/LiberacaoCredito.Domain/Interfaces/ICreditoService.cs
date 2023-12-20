using LiberacaoCredito.Domain.Models;

namespace LiberacaoCredito.Domain.Interfaces;

public interface ICreditoService
{
    public ResultadoCredito ProcessarCredito(Credito credito);
}