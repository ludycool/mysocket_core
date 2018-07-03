using jifan.ServerCenter.Common;
using System;

namespace jifan.ServerCenter.TR
{
    /// <summary>
    ///发送的数据包临时存档.处理完后清除
    /// </summary>
    public class SendData
    {
        /// <summary>
        /// 标识 源mac+seq
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        ///发送的 数据包(已拆)
        /// </summary>
        public Packet Pdata { get; set; }

        /// <summary>
        /// 数据包 字节包
        /// </summary>
        public byte[] Data { get; set; }


        /// <summary>
        /// 发送时间
        /// </summary>
        public DateTime SendTime { get; set; }

        /// <summary>
        /// 发送次数
        /// </summary>
        public int SendCount { get; set; }
    }
}
