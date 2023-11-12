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

        services.AddMassTransit(x =>
        {
            x.UsingAzureServiceBus((context, cfg) =>
            {
                cfg.Host(conexao);
                cfg.SubscriptionEndpoint("sub-1", "topico", e =>
                {
                    e.Consumer<PedidoCriadoConsumidor>();
                });
            });
        });
    })
    .Build();

host.Run();
