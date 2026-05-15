using SolucaoBarbearia.infra.Repositorios;
using SolucaoBarbearia.dominio.Interfaces;
using FluentValidation;
using FluentValidation.AspNetCore;
using Barbearia.Service.Validators;
using SolucaoBarbearia.servico.Interfaces;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddScoped<IClienteRepository, ClienteRepositorio>();
builder.Services.AddScoped<IClienteService, ClienteService>();

builder.Services.AddScoped<IAgendamentoRepository, AgendamentoRepositorio>();
builder.Services.AddScoped<IAgendamentoService, AgendamentoService>();
builder.Services.AddScoped<AgendamentoService>();

builder.Services.AddScoped<IProfissionalService, ProfissionalService>();
builder.Services.AddScoped<IProfissionalRepository, ProfissionalRepositorio>();
builder.Services.AddScoped<ProfissionalService>();
builder.Services.AddScoped<IServicoService, ServicoService>();
builder.Services.AddScoped<IServicoRepository, ServicoRepositorio>();
builder.Services.AddScoped<ServicoService>();
builder.Services.AddScoped<ServicoLojaRepositorio>();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<ClienteCadastroDtoValidator>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Barbearia API", Version = "v1" });
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

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }
