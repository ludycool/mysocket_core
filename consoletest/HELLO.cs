using SuperSocket.SocketBase.Command;
using SuperSocket.SocketBase.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace consoletest
{
    /// <summary>
    /// 自定义命令类HELLO，继承CommandBase，并传入自定义连接类MySession
    /// </summary>
    /// <summary>
    /// 自定义命令类HELLO，继承CommandBase，并传入自定义连接类MySession
    /// </summary>
    public class HELLO : CommandBase<MySession, BinaryRequestInfo>
    {



        const int defPkgBufSize = 3072;
        static int socketType = -1;//1 tcp 0 udp

        /// <summary>
        /// 自定义执行命令方法，注意传入的变量session类型为MySession
        /// </summary>
        /// <param name="session"></param>
        /// <param name="requestInfo"></param>
        public override void ExecuteCommand(MySession session, BinaryRequestInfo requestInfo)
        {

            // Log4netHelper.Debug("HELLO.ExecuteCommand", "接收 数据16进制：" + StringHelper.byteToHexStr(requestInfo.Body));
            // Log4netHelper.Debug("HELLO.ExecuteCommand", "接收 数据字符：" + System.Text.Encoding.ASCII.GetString(requestInfo.Body));


            #region  日志可删
            // Log4netHelper.Debug("HELLO.ExecuteCommand", "接收到数据为：" + requestInfo.Body.ToHexString());
            #endregion
            session.TrySend(requestInfo.Body);
        }
    }
}
