using Liberacao.Infra.CrossCutting.IoC;
using LiberacaoCredito.Api.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiConfiguration();
builder.Services.AddSwaggerConfiguration();

builder.Services.ResolveDependencies();

var app = builder.Build();

app.UseApiConfiguration();
app.UseSwaggerConfiguration(app.Environment);

app.Run();