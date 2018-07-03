using SuperWebSocket;
using SuperWebSocket.SubProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.Concurrent;
using jifan.ServerCenter.TR;
using SuperSocket.SocketBase.Config;

namespace consoletest.websocket
{
    public class MywebServer
    {
        public static WebSocketServer server;

        public MywebServer()
        {
            server = new WebSocketServer();
            server.NewSessionConnected += server_NewSessionConnected;
            server.NewDataReceived += server_NewDataReceived;
            server.NewMessageReceived += server_NewMessageReceived;
            server.SessionClosed += server_SessionClosed;
        }     /// <summary>
              /// 开启服务 ip是127.0.0.1 端口读配置文件
              /// </summary>
        public void startServer(string ServerIp, int post)
        {
            try
            {
                server.Setup(ServerIp, post);//设置端口
                server.Start();//开启监听


               // Log4netHelper.Info(typeof(WebTcpServer).FullName, "启动 WebTcpServer服务成功!");
            }
            catch (Exception ex)
            {
              //  Log4netHelper.Error(typeof(WebTcpServer).FullName, "启动服务失败", ex);
                Console.WriteLine(ex.Message);
            }
        }
        public void startServer(IServerConfig _config)
        {
            server.Setup(new RootConfig(), _config);//设置端口
            server.Start();//开启监听

        }
        #region superWebSocket 重写的方法


        /// <summary>
        /// 关闭连接
        /// </summary>
        /// <param name="session"></param>
        /// <param name="value"></param>
        void server_SessionClosed(WebSocketSession session, SuperSocket.SocketBase.CloseReason value)
        {
            Console.WriteLine(session.Origin);
            RemoveSession(session);//移除连接
        }
        /// <summary>
        /// 收到webSocket的数据
        /// </summary>
        /// <param name="session"></param>
        /// <param name="value"></param>
        void server_NewMessageReceived(WebSocketSession session, string value)
        {
            try
            {
                // Console.WriteLine(value);
                session.Send(value);
               // Log4netHelper.Debug("server_NewMessageReceived", "数据：" + value);

                //  RequstMode request = JsonHelper.FromJson<RequstMode>(value);
                //projectCommand(session, request);

            }
            catch (Exception ex)
            {
                Type classt = typeof(MyServer);
                string typeName = classt.ToString();//空间名.类名
                string tname = new System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name;//方法名
                string Title = typeName + "." + tname;
              //  Log4netHelper.Error(Title, "server_NewMessageReceived处理收至数据出错", ex);
            }
        }
        void server_NewDataReceived(WebSocketSession session, byte[] value)
        {
            // Console.WriteLine(value);
            // session.Send(value, 0, value.Length);
            WebCmd.ExecuteCommand(session, value);
           // Log4netHelper.Debug("server_NewDataReceived", "数据：" + value.ToHexString());
        }
        /// <summary>
        /// 新连接
        /// </summary>
        /// <param name="session"></param>
        void server_NewSessionConnected(WebSocketSession session)
        {

            Console.WriteLine(session.Origin);

        }


        #endregion

        #region udp 接收数据维护

        /// <summary>
        /// 接收的数据包 id=mac+seq 防止重接收
        /// </summary>
        internal static ConcurrentDictionary<string, DateTime> ReceiveId = new ConcurrentDictionary<string, DateTime>();

        /// <summary>
        /// 获取接收到的数据包 id
        /// </summary>
        /// <param name="Id"></param>
        static internal List<KeyValuePair<string, DateTime>> GetReceiveId()
        {

            return ReceiveId.ToList();
        }



        /// <summary>
        /// 移除接收到的数据包 id
        /// </summary>
        /// <param name="Id"></param>
        static internal void RemoveReceiveId(string Id)
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
        static internal bool PushReceiveId(string Id)
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
        static internal bool IsHaveReceiveId(string Id)
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
        static internal void EnqueueSendDatas(SendData SendData)
        {

            SendDatas.Enqueue(SendData);
        }
        /// <summary>
        /// 从头部 移除并 返回一个 使用前要判断是否数量>0
        /// </summary>
        /// <returns></returns>
        static internal SendData DequeueSendDatas()
        {

            return SendDatas.Dequeue();
        }
        /// <summary>
        /// 获取个数
        /// </summary>
        /// <returns></returns>
        static internal int GetSendDatasCount()
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
        static internal List<KeyValuePair<string, DateTime>> GetSendId()
        {

            return SendId.ToList();

        }


        /// <summary>
        /// 移除的发送数据包 id
        /// </summary>
        /// <param name="Id"></param>
        static internal void RemoveSendId(string Id)
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
        static internal bool PushSendId(string Id)
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
        static internal bool IsHaveSendId(string Id)
        {
            return SendId.Keys.Contains(Id);

        }


        #endregion
        #region 连接维护 属性

        /// <summary>
        /// 连接池，新连接加入，断连接，移除
        /// </summary>
        internal static ConcurrentDictionary<string, WebSocketSession> SessionCache = new ConcurrentDictionary<string, WebSocketSession>();

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
        static internal void RemoveSession(WebSocketSession token)
        {
            WebSocketSession tem1 = null;
            if (!string.IsNullOrEmpty(token.Id))
            {
                if (SessionCache.Keys.Contains(token.Id))
                {
                    // var tem = SessionCache[token.Id];
                    token.isLogin = false;
                    SessionCache.TryRemove(token.Id, out tem1);

                }
            }

        }
        /// <summary>
        /// 加入在线列表，如果已经存在，就更新
        /// </summary>
        /// <param name="token"></param>
        static internal void PushSession(WebSocketSession token)
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
        static internal bool IsOnline(string Id)
        {

            return SessionCache.Keys.Contains(Id);
        }
        /// <summary>
        /// 获取连接
        /// </summary>
        /// <param name="DeviceId"></param>
        /// <returns></returns>
        static internal WebSocketSession GetSessionId(String Id)
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
