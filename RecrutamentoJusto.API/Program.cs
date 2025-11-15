using Microsoft.EntityFrameworkCore;
using RecrutamentoJusto.API.Data;
using Asp.Versioning;


var builder = WebApplication.CreateBuilder(args);

// === CONFIGURAÇÃO DO ENTITY FRAMEWORK CORE E SQL SERVER ===
// Registra o DbContext e configura a conexão com o banco de dados SQL Server
// usando a string de conexão definida em appsettings.json
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.

builder.Services.AddControllers();

// ===== CONFIGURAÇÃO DO VERSIONAMENTO DA API =====
// Implementa versionamento de API permitindo múltiplas versões (v1, v2, etc)
// Formato: /api/v1/controller e /api/v2/controller
builder.Services.AddApiVersioning(options =>
{
    // Define v1.0 como versão padrão quando não especificada
    options.DefaultApiVersion = new Asp.Versioning.ApiVersion(1, 0);
    // Assume a versão padrão quando o cliente não especificar
    options.AssumeDefaultVersionWhenUnspecified = true;
    // Informa as versões suportadas no header da resposta
    options.ReportApiVersions = true;
}).AddApiExplorer(options =>
{
    // Formato do grupo de versão para URLs
    options.GroupNameFormat = "'v'VVV";
    // Substitui a versão no template da rota
    options.SubstituteApiVersionInUrl = true;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
