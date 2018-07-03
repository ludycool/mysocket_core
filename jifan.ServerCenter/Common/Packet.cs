using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jifan.ServerCenter.Common
{

    /* 修改日志
     去掉 procode 
 version cmd 共用一位
添加 pkgID

pload
去掉pnd,pakid,len

     */


    /// <summary>
    /// 接收到的数据包
    /// </summary>
    public class Packet
    {

        /// <summary>
        /// FFFF 头 0-1
        /// </summary>
        public byte[] head { set; get; }



        /// <summary>
        /// 长度 2位 procode到结束的和检验 2-3
        /// </summary>
        public UInt16 checksum { set; get; }



        /// <summary>
        /// 版本号 数据包索引4 高3位  bit7：5
        /// </summary>
        public byte version { set; get; }

        /// <summary>
        /// 是否需要ack 标识 1需要0不需要 包索引4 1bit  bit4 
        /// </summary>
        public byte AckFlag
        {
            set;
            get;
        }

        /// <summary>
        /// 命令 数据包索引4 低4位  4bit cmd	bit3：0
        /// </summary>
        public byte cmd { set; get; }


        /// <summary>
        /// 通信半径 5
        /// </summary>
        public byte radio { set; get; }
        /// <summary>
        /// 目标mac 8位 6-13
        /// </summary>
        public byte[] dstAddr { set; get; }
        /// <summary>
        /// 源mac 8位 14-21
        /// </summary>
        public byte[] srcAddr { set; get; }



        /// <summary>
        /// 序列号 2位 22-23
        /// </summary>
        public byte[] seq { set; get; }


        /// <summary>
        /// 分包 序号 24-25
        /// </summary>
        public byte[] pkgID { set; get; }


        /// <summary>
        /// 长度 2位 数据payload的长度 26-27
        /// </summary>
        public UInt16 dlen { set; get; }


        /// <summary>
        /// 数据承载层 28+
        /// </summary>
        public byte[] Payload { set; get; }

        /// <summary>
        /// 所有的数据
        /// </summary>
        public byte[] data { set; get; }
        public Packet(byte[] _data)
        {
            data = _data;
            //version = PacketHelper.GetVersion(data);
            //AckFlag = PacketHelper.GetAckFlag(data);
            //cmd = PacketHelper.GetCmd(data);
            //radio = PacketHelper.GetRadio(data);
            //seq = PacketHelper.GetSeq(data);
            //dlen = PacketHelper.Getdlen(data);
            //dstAddr = PacketHelper.GetDstAddr(data);
            //srcAddr = PacketHelper.GetSrcAddr(data);
            //Payload = PacketHelper.GetPayload(data);
            //pkgID = PacketHelper.GetpkgID(data);
        }
        public Packet()
        {
            pkgID = new byte[2];
        }

    }
}
