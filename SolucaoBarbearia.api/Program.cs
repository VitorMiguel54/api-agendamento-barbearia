using SolucaoBarbearia.infra.Repositorios;
using SolucaoBarbearia.dominio.Interfaces;
using FluentValidation;
using FluentValidation.AspNetCore;
using Barbearia.Service.Validators;
using SolucaoBarbearia.servico.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.OpenApi.Models;
using SolucaoBarbearia.api.Security;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddScoped<IClienteRepository, ClienteRepositorio>();
builder.Services.AddScoped<IClienteService, ClienteService>();

builder.Services.AddScoped<IAgendamentoRepository, AgendamentoRepositorio>();
builder.Services.AddScoped<IAgendamentoService, AgendamentoService>();

builder.Services.AddScoped<IProfissionalService, ProfissionalService>();
builder.Services.AddScoped<IProfissionalRepository, ProfissionalRepositorio>();
builder.Services.AddScoped<IServicoService, ServicoService>();
builder.Services.AddScoped<IServicoRepository, ServicoRepositorio>();
builder.Services.AddScoped<IServicoLojaRepository, ServicoLojaRepositorio>();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<ClienteCadastroDtoValidator>();

builder.Services.AddControllers();
builder.Services
    .AddAuthentication(ApiKeyAuthenticationHandler.SchemeName)
    .AddScheme<AuthenticationSchemeOptions, ApiKeyAuthenticationHandler>(
        ApiKeyAuthenticationHandler.SchemeName,
        options => { });
builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Barbearia API", Version = "v1" });
    c.AddSecurityDefinition(ApiKeyAuthenticationHandler.SchemeName, new OpenApiSecurityScheme
    {
        Description = "Informe a API key no header X-Api-Key.",
        Name = "X-Api-Key",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = ApiKeyAuthenticationHandler.SchemeName
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = ApiKeyAuthenticationHandler.SchemeName
                }
            },
            Array.Empty<string>()
        }
    });
});

var tipoRepositorio = builder.Configuration["RepositoryConfig:Tipo"];

builder.Services.AddScoped<ILojaService, LojaService>();
builder.Services.AddScoped<ILojaRepository, LojaRepositorio>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Barbearia API v1"));
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }
