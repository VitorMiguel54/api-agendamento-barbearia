using SolucaoBarbearia.infra.Repositorios;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddScoped<ClienteRepositorio>();
builder.Services.AddScoped<ClienteService>();

builder.Services.AddScoped<AgendamentoRepositorio>();
builder.Services.AddScoped<AgendamentoService>();

builder.Services.AddScoped<ProfissionalService>();
builder.Services.AddScoped<ProfissionalRepositorio>();

builder.Services.AddScoped<ServicoService>();
builder.Services.AddScoped<ServicoRepositorio>();

builder.Services.AddScoped<LojaRepositorio>();
builder.Services.AddScoped<LojaService>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();