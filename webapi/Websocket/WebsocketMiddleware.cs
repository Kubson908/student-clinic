using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NuGet.Protocol;
using Przychodnia.Shared;
using System.IdentityModel.Tokens.Jwt;
using System.Net.WebSockets;
using System.Security.Claims;
using System.Text.Json;

namespace Przychodnia.Webapi.Websocket
{
    public class WebSocketMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<WebSocketsController> _logger;
        List<WsConnectionObject> connections;
        JwtSecurityTokenHandler handler;
        List<string> endpoints = new List<string>()
        {
            "/api/appointment/create",
            "/api/appointment/assign-appointment/",
            "/api/appointment/cancel-appointment/"
        };


        public WebSocketMiddleware(RequestDelegate next, ILogger<WebSocketsController> logger)
        {
            _next = next;
            connections = new List<WsConnectionObject>();
            _logger = logger;
            handler = new JwtSecurityTokenHandler();
        }

        public async Task InvokeAsync(HttpContext context)
        {
            CancellationToken ct = context.RequestAborted;
            if (endpoints.Any(e => context.Request.Path.ToString().Contains(e))  
                && context.Response.Body != null)
            {
                var originalBody = context.Response.Body;
                try
                {
                    var memoryBodyStream = new MemoryStream();

                    context.Response.Body = memoryBodyStream;
                    await _next.Invoke(context);
                    memoryBodyStream.Seek(0, SeekOrigin.Begin);
                    string body = await new StreamReader(memoryBodyStream).ReadToEndAsync();
                    memoryBodyStream.Seek(0, SeekOrigin.Begin);

                    await memoryBodyStream.CopyToAsync(originalBody);
                    if (context.Response.StatusCode >= 300) return;
                    JObject json = JObject.Parse(body);

                    switch (json["message"]?.ToString())
                    {
                        case "Appointment created":
                            await SendEventAsync(new List<string>{"Staff"}, json["data"]?.ToString() ?? "", "newAppointment", ct);
                            break;

                        case "Appointment cancelled": // ???
                            await SendEventAsync(new List<string> {"Staff"}, json["data"]?.ToString() ?? "", "deletedAppointment", ct);
                            break;
                        
                        case "Appointment assigned": // ???
                            await SendAssignedAppointment(json["data"]?.ToString() ?? "", "assignedAppointment", ct);
                            break;
                    }
                        
                    return;

                }
                finally
                {
                    context.Response.Body = originalBody;
                }
            }
            if (!context.WebSockets.IsWebSocketRequest)
            {
                await _next.Invoke(context);
                return;
            }
            string wsId = context.Connection.Id;
            
            
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
                    con.Role = token.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();
                    con.UserId = token.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
                    continue;
                }
                Console.WriteLine(wsId + ": " + data);
                foreach (var item in connections.Where(c => c.Role != null && c.Role.Contains("Staff")))
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
                try
                {
                    await ws.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, "UserDisconnected", ct);
                }  
                catch (TaskCanceledException) { } catch (OperationCanceledException) { } catch (WebSocketException) { }
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


                ms.Seek(0, SeekOrigin.Begin);


                if (receiveResult.MessageType != WebSocketMessageType.Text)
                    return string.Empty;

                using (StreamReader reader = new StreamReader(ms, System.Text.Encoding.UTF8))
                {
                    return await reader.ReadToEndAsync();
                }

            }
        }

        Task SendStringAsync(WebSocket ws, string data, CancellationToken ct = default)
        {
            var buffer = System.Text.Encoding.UTF8.GetBytes(data);
            var segment = new ArraySegment<byte>(buffer);
            return ws.SendAsync(segment, WebSocketMessageType.Text, true, ct);
        }

        public async Task SendEventAsync(List<string> role, string data, string eventName, CancellationToken ct = default)
        {
            var obj = new WebSocketEventDto()
            {
                EventName = eventName,
                Data = JObject.Parse(data)
            };
            var buffer = System.Text.Encoding.UTF8.GetBytes(obj.ToJson() ?? "");
            var segment = new ArraySegment<byte>(buffer);
            
            foreach (var item in connections.Where(c => c.Role != null && c.Role.Any(r => role.Contains(r.ToString()))))
            {
                if (item.Connection != null && item.Connection.State != WebSocketState.Open ||
                    item.Connection == null)
                {
                    continue;
                }

                await item.Connection.SendAsync(segment, WebSocketMessageType.Text, true, ct);
            }
        }

        public async Task SendAssignedAppointment(string data, string eventName, CancellationToken ct = default)
        {
            var obj = new WebSocketEventDto()
            {
                EventName = eventName,
                Data = JObject.Parse(data)
            };
            var buffer = System.Text.Encoding.UTF8.GetBytes(obj.ToJson() ?? "");
            var segment = new ArraySegment<byte>(buffer);
            dynamic temp = JsonConvert.DeserializeObject(data) ?? "";
            string patientId = (string)temp.patientId;
            string doctorId = (string)temp.doctorId;
            Console.WriteLine(patientId + " | " + doctorId);
            foreach (var item in connections.Where(c => c.UserId == patientId || c.UserId == doctorId))
            {
                if (item.Connection != null && item.Connection.State != WebSocketState.Open ||
                    item.Connection == null)
                {
                    continue;
                }

                await item.Connection.SendAsync(segment, WebSocketMessageType.Text, true, ct);
            }
        }
    }
}
