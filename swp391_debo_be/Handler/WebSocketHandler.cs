using swp391_debo_be.Config.VnPay;
using swp391_debo_be.Services.Implements;
using swp391_debo_be.Services.Interfaces;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace swp391_debo_be.Handler
{
    public class WebSocketHandler
    {
        private readonly IPaymentService paymentService;
        public WebSocketHandler(IPaymentService paymentService)
        {
            this.paymentService = paymentService;
        }

        public async Task HandleAsync(HttpContext context, WebSocket webSocket)
        {
            var buffer = new byte[1024 * 4];
            WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

            while (!result.CloseStatus.HasValue)
            {
                var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                var vnpayResponse = JsonSerializer.Deserialize<VnpayPayResponse>(message);
                var response = paymentService.HandlePaymentResponse(vnpayResponse);

                var responseMessage = JsonSerializer.Serialize(response);
                var responseBytes = Encoding.UTF8.GetBytes(responseMessage);

                await webSocket.SendAsync(new ArraySegment<byte>(responseBytes, 0, responseBytes.Length), result.MessageType, result.EndOfMessage, CancellationToken.None);

                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            }

            await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        }
    }
}
