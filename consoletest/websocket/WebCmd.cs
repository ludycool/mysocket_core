using SuperWebSocket;
using System;
using System.Collections.Generic;
using System.Text;

namespace consoletest.websocket
{
    public class WebCmd
    {
        public static void ExecuteCommand(WebSocketSession session, byte[] data)
        {

            try
            {
                session.TrySend(data);

            }
            catch (Exception ex)
            {
     
            }

        }
    }
}
