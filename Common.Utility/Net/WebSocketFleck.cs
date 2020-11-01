using System;
using System.Threading.Tasks;
using Fleck;

namespace Common.Utility.Net
{
    public class WebSocketFleck: IDisposable
    {
        private  WebSocketServer server;

        public WebSocketFleck(string location = "ws://0.0.0.0:8080", bool supportDualStack = true)
        {
            server = new WebSocketServer(location, supportDualStack);
            server.Start(socket =>
            {
                socket.OnOpen = ()=>socket.Send("AESCR:" + "Open!"); ;
                socket.OnClose = () => socket.Send("AESCR:" + "Close!"); 
                socket.OnMessage = message => socket.Send("AESCR:"+message);
            });
        }

        public void Dispose()
        {
            server.Dispose();
        }
    }
}
