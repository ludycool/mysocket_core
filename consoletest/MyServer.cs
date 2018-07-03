using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Config;
using SuperSocket.SocketBase.Protocol;
using System;
using System.Collections.Concurrent;

namespace consoletest
{
    /// <summary>
    /// 自定义服务器类MyServer，继承AppServer，并传入自定义连接类MySession
    /// </summary>
    public class MyServer : AppServer<MySession, BinaryRequestInfo>
    {
        public MyServer()
            : base(new DefaultReceiveFilterFactory<BaseReceiveFilter, BinaryRequestInfo>())
        {

        }
        #region 自定义 属性
        /// <summary>
        /// 连接池，新连接加入，断连接，移除
        /// </summary>
        internal static ConcurrentDictionary<string, MySession> SessionCache = new ConcurrentDictionary<string, MySession>();

        /// <summary>
        /// 移除在线列表
        /// </summary>
        /// <param name="token"></param>
        internal void RemoveSession(MySession token)
        {

            MySession tem1 = null;
            if (SessionCache.Keys.Contains(token.Id))
            {
                var tem = SessionCache[token.Id];
                SessionCache.TryRemove(token.Id, out tem1);

            }

        }
        /// <summary>
        /// 加入在线列表，如果已经存在，就更新
        /// </summary>
        /// <param name="token"></param>
        internal void PushSession(MySession token)
        {
            if (!SessionCache.Keys.Contains(token.Id))
            {
                SessionCache.TryAdd(token.Id, token);

            }
            else
            {
                SessionCache[token.Id] = token;
            }
        }
        /// <summary>
        /// 获取连接
        /// </summary>
        /// <param name="DeviceId"></param>
        /// <returns></returns>
        internal MySession GetSessionId(String Id)
        {
            MySession ret = null;

            if (SessionCache.ContainsKey(Id))
            {
                ret = SessionCache[Id];
            }

            return ret;
        }

        #endregion

        #region 重写的方法

        protected override bool Setup(IRootConfig rootConfig, IServerConfig config)
        {
            return base.Setup(rootConfig, config);
        }

        protected override void OnStarted()
        {
            //LogHelper.WriteLog("WeChat服务启动");
            base.OnStarted();

        }

        protected override void OnStopped()
        {
            //LogHelper.WriteLog("WeChat服务停止");
            base.OnStopped();
        }

        /// <summary>
        /// 新的连接
        /// </summary>
        /// <param name="session"></param>
        protected override void OnNewSessionConnected(MySession session)
        {
            //LogHelper.WriteLog("WeChat服务新加入的连接:" + session.LocalEndPoint.Address.ToString());
            base.OnNewSessionConnected(session);
        }

        /// <summary>
        /// 断开连接
        /// </summary>
        /// <param name="session"></param>
        /// <param name="reason"></param>
        protected override void OnSessionClosed(MySession session, CloseReason reason)
        {

            RemoveSession(session);//从池中移除
            base.OnSessionClosed(session, reason);
        }
        #endregion
    }
}
