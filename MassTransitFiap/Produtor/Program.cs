using Core;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var configuration = builder.Configuration;
var servidor = configuration.GetSection("MassTransitAzure")["Conexao"] ?? string.Empty;

// Configurando o MassTransit para usar o RabbitMQ 
builder.Services.AddMassTransit(x =>
{
    x.UsingAzureServiceBus((context, cfg) =>
    {
        cfg.Host(servidor);
        cfg.Message<Pedido>(x => x.SetEntityName("topico"));//dessa maneira mudamos para um topico e não uma fila, no caso demos o nome de 'topico'. O ideal é colocar no appsettings.json
    });
});

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
