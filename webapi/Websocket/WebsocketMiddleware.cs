using System.IdentityModel.Tokens.Jwt;
using System.Net.WebSockets;
using System.Security.Claims;

namespace Przychodnia.Webapi.Websocket
{
    public class WebSocketMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<WebSocketsController> _logger;
        List<WsConnectionObject> connections;
        JwtSecurityTokenHandler handler;


        public WebSocketMiddleware(RequestDelegate next, ILogger<WebSocketsController> logger)
        {
            _next = next;
            connections = new List<WsConnectionObject>();
            _logger = logger;
            handler = new JwtSecurityTokenHandler();
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.WebSockets.IsWebSocketRequest)
            {
                await _next.Invoke(context);
                return;
            }
            string wsId = context.Connection.Id;
            
            
            CancellationToken ct = context.RequestAborted;
            WebSocket ws = await context.WebSockets.AcceptWebSocketAsync();
            connections.Add(new WsConnectionObject()
            {
                Id = wsId,
                Connection = ws,
            });
            _logger.Log(LogLevel.Information, wsId + ": WebSocket connection established" 
                + "\nTotal connections: " + connections.Count());

            while (true)
            {
                if (ct.IsCancellationRequested)
                {
                    return;
                }
                string data = string.Empty;
                try
                {
                    data = await ReadStringAsync(ws, ct);
                } catch (OperationCanceledException)
                {
                    break;
                }
                var con = connections.First(c => c.Id == wsId);
                if (string.IsNullOrEmpty(data))
                {
                    if (ws.State != WebSocketState.Open)
                    {
                        break;
                    }

                    continue;
                }
                if (!con.IsAuthenticated && handler.CanReadToken(data))
                {
                    var token = handler.ReadJwtToken(data);
                    con.IsAuthenticated = true;
                    con.Role = token.Claims.First(c => c.Type == ClaimTypes.Role).Value;
                    continue;
                }
                Console.WriteLine(wsId + ": " + data);
                foreach (var item in connections.Where(c => c.Role == "Patient"))
                {
                    if (item.Connection != null && item.Connection.State != WebSocketState.Open)
                    {
                        continue;
                    }

                    await SendStringAsync(item.Connection ?? ws, data, ct);
                }
            }
            connections.Remove(connections.First(c => c.Id == wsId));
            if (ws.State != WebSocketState.Aborted )
                await ws.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, "UserDisconnected", ct);
            _logger.Log(LogLevel.Information, wsId + ": WebSocket connection closed"
                            + "\nTotal connections: " + connections.Count());
            ws.Dispose();

            /*await _next(context);*/
        }

        async Task<string> ReadStringAsync(WebSocket ws, CancellationToken ct = default)
        {
            var buffer = new ArraySegment<byte>(new byte[1024 * 8]);

            using (MemoryStream ms = new MemoryStream())
            {
                WebSocketReceiveResult receiveResult;

                do
                {
                    ct.ThrowIfCancellationRequested();

                    receiveResult = await ws.ReceiveAsync(buffer, ct);

                    if (buffer.Array != null) ms.Write(buffer.Array, buffer.Offset, receiveResult.Count);

                } while (!receiveResult.EndOfMessage);


                ms.Seek(0, SeekOrigin.Begin); // Changing stream position to cover whole message


                if (receiveResult.MessageType != WebSocketMessageType.Text)
                    return string.Empty;

                using (StreamReader reader = new StreamReader(ms, System.Text.Encoding.UTF8))
                {
                    return await reader.ReadToEndAsync(); // decoding message
                }

            }
        }

        Task SendStringAsync(WebSocket ws, string data, CancellationToken ct = default)
        {
            var buffer = System.Text.Encoding.UTF8.GetBytes(data);
            var segment = new ArraySegment<byte>(buffer);
            return ws.SendAsync(segment, WebSocketMessageType.Text, true, ct);
        }

        Task SendEventAsync(WebSocket ws, string data, CancellationToken ct = default)
        {
            var buffer = System.Text.Encoding.UTF8.GetBytes(data);
            var segment = new ArraySegment<byte>(buffer);
            return ws.
        }

      /*  async Task SendEvent(string name, string data, CancellationToken ct = default)
        {
            var buffer = System.Text.Encoding.UTF8.GetBytes(data);
            var segment = new ArraySegment<byte>(buffer);
            foreach (var item in connections)
            {
                if (item.Value.State != WebSocketState.Open)
                {
                    continue;
                }

                await item.Value.SendAsync(segment, WebSocketMessageType.Binary);
            }
        }*/
    }
}
