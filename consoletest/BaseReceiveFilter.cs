using System;
using SuperSocket.Common;
using SuperSocket.SocketBase.Protocol;


namespace consoletest
{
    /// <summary>
    /// 不过滤，获取所有的
    /// </summary>
    class BaseReceiveFilter : ReceiveFilterBase<BinaryRequestInfo>
    {
        public BaseReceiveFilter()
            : base()
        {

        }

        public override BinaryRequestInfo Filter(byte[] readBuffer, int offset, int length, bool toBeCopied, out int rest)
        {
            rest = 0;
            return new BinaryRequestInfo("HELLO", readBuffer.CloneRange(offset, length));// hello为cmd
        }
    }
}
