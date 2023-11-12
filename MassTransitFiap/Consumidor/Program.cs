using Consumidor;
using Consumidor.Eventos;
using MassTransit;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        var configuration = hostContext.Configuration;
        var servidor = configuration.GetSection("MassTransit")["Servidor"] ?? string.Empty;
        var usuario = configuration.GetSection("MassTransit")["Usuario"] ?? string.Empty;
        var senha = configuration.GetSection("MassTransit")["Senha"] ?? string.Empty;
        var fila = configuration.GetSection("MassTransit")["NomeFila"] ?? string.Empty;
        services.AddHostedService<Worker>();

        services.AddMassTransit(x =>
        {
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(servidor, "/", h =>
                {
                    h.Username(usuario);
                    h.Password(senha);
                });

                cfg.ReceiveEndpoint(fila, e =>
                {
                    e.Consumer<PedidoCriadoConsumidor>(context);
                });

                //se precisar criar outro consumidor, basta adicionar aqui e criar a classe

                cfg.ConfigureEndpoints(context);
            });

            x.AddConsumer<PedidoCriadoConsumidor>();
            //se precisar criar outro consumidor, basta adicionar aqui e criar a classe
        });
    })
    .Build();

host.Run();
