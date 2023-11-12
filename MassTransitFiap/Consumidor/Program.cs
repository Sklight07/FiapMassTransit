using Consumidor;
using Consumidor.Eventos;
using MassTransit;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        var configuration = hostContext.Configuration;
        var conexao = configuration.GetSection("MassTransitAzure")["Conexao"] ?? string.Empty;
        var fila = configuration.GetSection("MassTransitAzure")["NomeFila"] ?? string.Empty;
        services.AddHostedService<Worker>();

        _ = services.AddMassTransit(x =>
        {
            x.UsingAzureServiceBus((context, cfg) =>
            {
                cfg.Host(conexao);
                cfg.ReceiveEndpoint(fila, e =>
                {
                    e.Consumer<PedidoCriadoConsumidor>();
                    //e.UseRetry(r => r.Immediate(5)); // caso de um problema ao tentar consumir a mensagem, tenta novamente 5 vezes
                });
            });
        });
    })
    .Build();

host.Run();
