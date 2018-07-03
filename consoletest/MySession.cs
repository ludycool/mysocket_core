using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace consoletest
{
   /// <summary>
    /// 自定义连接类MySession，继承AppSession，并传入到AppSession
    /// </summary>
    public class MySession : AppSession<MySession, BinaryRequestInfo>
    {

        /// <summary>
        /// 标识
        /// </summary>
       public string Id{set;get;}
        /// <summary>
        /// 新连接 udp第一次发送会执行一次  tcp 连接一次，执行一次
        /// </summary>
        protected override void OnSessionStarted()
        {

            //输出客户端IP地址
            Console.WriteLine(this.RemoteEndPoint.ToString());
            //输出客户端IP地址
            //Console.WriteLine(this.LocalEndPoint.Address.ToString());
            this.Send("\n\rHello User");
        }

        /// <summary>
        /// 未知的Command
        /// </summary>
        /// <param name="requestInfo"></param>
        protected override void HandleUnknownRequest(BinaryRequestInfo requestInfo)
        {
            this.Send("\n\r未知的命令");
        }

        /// <summary>
        /// 捕捉异常并输出
        /// </summary>
        /// <param name="e"></param>
        protected override void HandleException(Exception e)
        {
            this.Send("\n\r异常: {0}", e.Message);
        }

        /// <summary>
        /// 连接关闭
        /// </summary>
        /// <param name="reason"></param>
        protected override void OnSessionClosed(CloseReason reason)
        {
            base.OnSessionClosed(reason);

           
        }
    }
}
