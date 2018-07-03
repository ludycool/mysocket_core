
using jifan.ServerCenter.TR;
using SuperWebSocket;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace consoletest.websocket
{
    /// <summary>
    /// 在线连接列表
    /// </summary>
    internal class SocketSessionPool
    {

        public ConcurrentDictionary<string, WebSocketSession> SessionCache;

        public SocketSessionPool()
        {
            SessionCache = new ConcurrentDictionary<string, WebSocketSession>();
        }
        #region udp 接收数据维护

        /// <summary>
        /// 接收的数据包 id=mac+seq 防止重接收
        /// </summary>
        internal static ConcurrentDictionary<string, DateTime> ReceiveId = new ConcurrentDictionary<string, DateTime>();

        /// <summary>
        /// 获取接收到的数据包 id
        /// </summary>
        /// <param name="Id"></param>
        internal List<KeyValuePair<string, DateTime>> GetReceiveId()
        {

            return ReceiveId.ToList();
        }



        /// <summary>
        /// 移除接收到的数据包 id
        /// </summary>
        /// <param name="Id"></param>
        internal void RemoveReceiveId(string Id)
        {

            if (!string.IsNullOrEmpty(Id))
            {
                if (ReceiveId.Keys.Contains(Id))
                {
                    DateTime DD = DateTime.Now;
                    ReceiveId.TryRemove(Id, out DD);

                }
            }

        }

        /// <summary>
        /// 加入接收的数据,如果已经接收了,返回false 否则添加返回true
        /// </summary>
        /// <param name="token"></param>
        internal bool PushReceiveId(string Id)
        {

            if (!IsHaveReceiveId(Id))
            {
                ReceiveId.TryAdd(Id, DateTime.Now);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        internal bool IsHaveReceiveId(string Id)
        {
            return ReceiveId.Keys.Contains(Id);

        }


        #endregion

        #region udp 发送数据维护

        /// <summary>
        /// 发送的数据包
        /// </summary>
        internal static Queue<SendData> SendDatas = new Queue<SendData>();


        /// <summary>
        /// 从尾部加入一个
        /// </summary>
        /// <returns></returns>
        internal void EnqueueSendDatas(SendData SendData)
        {

            SendDatas.Enqueue(SendData);
        }
        /// <summary>
        /// 从头部 移除并 返回一个 使用前要判断是否数量>0
        /// </summary>
        /// <returns></returns>
        internal SendData DequeueSendDatas()
        {

            return SendDatas.Dequeue();
        }
        /// <summary>
        /// 获取个数
        /// </summary>
        /// <returns></returns>
        internal int GetSendDatasCount()
        {

            return SendDatas.Count();
        }


        /// <summary>
        /// 发送的数据包 id=mac+seq 防止重发
        /// </summary>
        internal static ConcurrentDictionary<string, DateTime> SendId = new ConcurrentDictionary<string, DateTime>();

        /// <summary>
        /// 获取接收到的数据包 id
        /// </summary>
        /// <param name="Id"></param>
        internal List<KeyValuePair<string, DateTime>> GetSendId()
        {

            return SendId.ToList();

        }


        /// <summary>
        /// 移除的发送数据包 id
        /// </summary>
        /// <param name="Id"></param>
        internal void RemoveSendId(string Id)
        {
            if (!string.IsNullOrEmpty(Id))
            {
                if (SendId.Keys.Contains(Id))
                {
                    DateTime tem = DateTime.Now;
                    SendId.TryRemove(Id, out tem);

                }
            }

        }

        /// <summary>
        /// 加入发送的数据,如果已经接收了,返回false 否则添加返回true
        /// </summary>
        /// <param name="token"></param>
        internal bool PushSendId(string Id)
        {
            if (!IsHaveSendId(Id))
            {
                SendId.TryAdd(Id, DateTime.Now);
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 是否有
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        internal bool IsHaveSendId(string Id)
        {
            return SendId.Keys.Contains(Id);

        }


        #endregion
        #region 连接维护 属性

        public int Count
        {
            get
            {
                return SessionCache.Count;
            }
        }
        /// <summary>
        /// 移除在线列表
        /// </summary>
        /// <param name="token"></param>
        internal void RemoveSession(WebSocketSession token)
        {
            WebSocketSession tem1 = null;
            if (!string.IsNullOrEmpty(token.Id))
            {
                if (SessionCache.Keys.Contains(token.Id))
                {
                    // var tem = SessionCache[token.Id];
                    SessionCache.TryRemove(token.Id, out tem1);

                }
            }

        }
        /// <summary>
        /// 加入在线列表，如果已经存在，就更新
        /// </summary>
        /// <param name="token"></param>
        internal void PushSession(WebSocketSession token)
        {
            if (!string.IsNullOrEmpty(token.Id))
            {
                if (!SessionCache.Keys.Contains(token.Id))
                {
                    SessionCache.TryAdd(token.Id, token);

                }
                else
                {
                    if (SessionCache[token.Id] != token)
                    {
                        SessionCache[token.Id].Id = "";
                        SessionCache[token.Id].Close();
                    }
                    SessionCache[token.Id] = token;
                }
            }
        }
        /// <summary>
        /// 是否在线 是否有连接
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        internal bool IsOnline(string Id)
        {

            return SessionCache.Keys.Contains(Id);
        }
        /// <summary>
        /// 获取连接
        /// </summary>
        /// <param name="DeviceId"></param>
        /// <returns></returns>
        internal WebSocketSession GetSessionId(String Id)
        {
            WebSocketSession ret = null;

            if (SessionCache.ContainsKey(Id))
            {
                ret = SessionCache[Id];
            }
            return ret;
        }

        #endregion
    }


}
