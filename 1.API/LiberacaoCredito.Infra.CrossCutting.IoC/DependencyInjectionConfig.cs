using LiberacaoCredito.Application.Services;
using LiberacaoCredito.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace LiberacaoCredito.Infra.CrossCutting.IoC;

public static class DependencyInjectionConfig
{
    public static void ResolveDependencies(this IServiceCollection services)
    {
        services.AddScoped<ICreditoService, CreditoService>();
    }
}