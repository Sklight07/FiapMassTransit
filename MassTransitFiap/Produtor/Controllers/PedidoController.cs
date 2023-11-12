using Core;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Produtor.Controllers
{
    [ApiController]
    [Route("/Pedido")]
    public class PedidoController : ControllerBase
    {
        private readonly IBus _bus;
        private readonly IConfiguration _configuration;

        public PedidoController(IBus bus, IConfiguration configuration)
        {
            _bus = bus;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> Post()
        {
            /*var nomeFila = _configuration.GetSection("MassTransitAzure")["NomeFila"] ?? string.Empty;
            var endPoint = await _bus
                .GetSendEndpoint(new Uri($"queue:{nomeFila}"));

            await endPoint.Send(new Pedido(1, new Usuario(2, "Leandro", "leandro@gmail.com")));*/

            await _bus.Publish(new Pedido(1, new Usuario(3, "Leandro", "leandro@gmail.com")));//aqui estamos mandando para um topico, é importante lembrar que ao menos 1 consumidor precisa estar escutando esse topico se não o azure vai descartar a mensagem

            return Ok();
        }
    }
}
