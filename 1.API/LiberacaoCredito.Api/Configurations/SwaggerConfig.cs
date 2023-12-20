using Microsoft.OpenApi.Models;

namespace LiberacaoCredito.Api.Configurations;

public static class SwaggerConfig
{
    public static void AddSwaggerConfiguration(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Liberação Crédito API",
                Version = "v1",
                Contact = new OpenApiContact { Name = "João Paulo Fontes", Email = "vasconcelosjoao438@gmail.com" }
            });
        });
    }

    public static void UseSwaggerConfiguration(this IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Liberação Crédito API"); });

        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
    }
}