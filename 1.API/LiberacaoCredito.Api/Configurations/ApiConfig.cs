using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace LiberacaoCredito.Api.Configurations;

public static class ApiConfig
{
    public static void AddApiConfiguration(this IServiceCollection services)
    {
        services.AddControllers();

        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });
    }
    
    public static void UseApiConfiguration(this IApplicationBuilder app)
    {
        app.UseRouting();
        app.UseHttpsRedirection();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}