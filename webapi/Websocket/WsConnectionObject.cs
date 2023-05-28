using System.Net.WebSockets;

namespace Przychodnia.Webapi.Websocket
{
    public class WsConnectionObject
    {
        public string? Id { get; set; }
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public WebSocket Connection { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public bool IsAuthenticated { get; set; } = false;
        public string? Role { get; set; }
    }
}
