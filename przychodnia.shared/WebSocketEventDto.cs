using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Przychodnia.Shared
{
    public class WebSocketEventDto
    {
        public string EventName { get; set; } = string.Empty;
        public Object? Data { get; set; }
    }
}
