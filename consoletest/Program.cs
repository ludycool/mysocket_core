using consoletest.websocket;
using Microsoft.Extensions.Configuration;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Config;
using SuperSocket.SocketEngine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace consoletest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            #region 硬编码 启动
            
            var appServer = new MyServer();
            IServerConfig m_Config;

            m_Config = new ServerConfig
            {
                Port = jifanConfig.port, //服务器端口
                Ip = "Any",
                MaxConnectionNumber = jifanConfig.maxConnectionNumber,
                Mode = jifanConfig.mode,//tcp udp
                MaxRequestLength = jifanConfig.maxRequestLength,
                SendingQueueSize = 20,//发送队列大小
                Name = jifanConfig.name,
                ClearIdleSession = jifanConfig.clearIdleSession,//是否清空 空闲会话
                IdleSessionTimeOut = jifanConfig.IdleSessionTimeOut,//空闲会话 超时时间
                LogBasicSessionActivity = jifanConfig.logBasicSessionActivity,//是否记录session的基本活动，如连接和断开;
                LogAllSocketException = jifanConfig.logAllSocketException
            };

            //设置服务监听端口
            if (!appServer.Setup(new RootConfig(), m_Config))
            {
                Console.WriteLine("端口设置失败!");
                Console.ReadKey();
                return;
            }

            //启动服务
            if (!appServer.Start())
            {
                Console.WriteLine("启动服务失败!");
                Console.ReadKey();
                return;
            }
           
            #endregion

            #region  bootstrap 启动

            /*
            var bootstrap = BootstrapFactory.CreateBootstrap();

            if (!bootstrap.Initialize())
            {
                Console.WriteLine("Failed to initialize!");
                Console.ReadKey();
                return;
            }

            var result = bootstrap.Start();

            Console.WriteLine("Start result: {0}!", result);

            if (result == StartResult.Failed)
            {
                Console.WriteLine("Failed to start!");
                Console.ReadKey();
                return;
            }
            */

            #endregion

            #region websocket


            MywebServer appwebServer = new MywebServer();
            IServerConfig web_Config;

            //CertificateConfig _Certificate =new CertificateConfig();//http证书
            //_Certificate.FilePath = "socket.glalaxy.com.pfx";
            //_Certificate.Password = "123456";
            web_Config = new ServerConfig
            {
                Port = 3344, //服务器端口
                Ip = "Any",
                MaxConnectionNumber = jifanConfig.maxConnectionNumber,
                Mode = SocketMode.Tcp,//tcp udp
                MaxRequestLength = jifanConfig.maxRequestLength,
                SendingQueueSize = 20,//发送队列大小
                Name = "websocket",
                ClearIdleSession = jifanConfig.clearIdleSession,//是否清空 空闲会话
                IdleSessionTimeOut = jifanConfig.IdleSessionTimeOut,//空闲会话 超时时间
                LogBasicSessionActivity = jifanConfig.logBasicSessionActivity,//是否记录session的基本活动，如连接和断开;
                LogAllSocketException = jifanConfig.logAllSocketException
                //,Security = "tls"
                //, Certificate = _Certificate
            };
            appwebServer.startServer(web_Config);

            #endregion
            Console.WriteLine("启动服务成功，输入exit退出!");

            while (true)
            {
                var str = Console.ReadLine();
                if (str.ToLower().Equals("exit"))
                {
                   // appServer.Stop();

                    break;
                }
            }

            Console.WriteLine();

            //停止服务
            //  appServer.Stop();
            //Stop the appServer

            Console.WriteLine("服务已停止，按任意键退出!");

            Console.ReadLine();
        }
    }

    class jifanConfig
    {

        #region supersocket 常 用 基本配置，如需更多，请参考添加
        static List<IConfigurationSection> _Servers;
        public static List<IConfigurationSection> Servers
        {
            get
            {
                if (_Servers == null)
                {
                    IConfigurationSection serverroot = ConfigurationManager.GetSection("supersocket:servers");
                    _Servers = serverroot.GetChildren().ToList();
                }
                return _Servers;
            }
        }

        public static int port
        {
            get
            {
                return int.Parse(Servers[0]["port"]);
            }
        }
        public static string name
        {
            get
            {
                if (Servers[0]["name"] != null)
                {
                    return Servers[0]["name"];
                }
                return "myserver";
            }
        }

        public static SocketMode mode
        {
            get
            {
                if (Servers[0]["mode"] != null && Servers[0]["mode"].ToLower().Equals("udp"))
                {
                    return SocketMode.Udp;
                }
                return SocketMode.Tcp;
            }
        }

        public static int maxConnectionNumber
        {
            get
            {
                if (Servers[0]["maxConnectionNumber"] != null)
                {
                    return int.Parse(Servers[0]["maxConnectionNumber"]);
                }
                return 30000;
            }
        }
        public static bool clearIdleSession
        {
            get
            {
                if (Servers[0]["clearIdleSession"] != null)
                {
                    return bool.Parse(Servers[0]["clearIdleSession"]);
                }
                return false;
            }
        }
        public static int IdleSessionTimeOut
        {
            get
            {
                if (Servers[0]["IdleSessionTimeOut"] != null)
                {
                    return int.Parse(Servers[0]["IdleSessionTimeOut"]);
                }
                return 120;
            }
        }
        public static int maxRequestLength
        {
            get
            {
                if (Servers[0]["maxRequestLength"] != null)
                {
                    return int.Parse(Servers[0]["maxRequestLength"]);
                }
                return 3072;
            }
        }
        public static bool logBasicSessionActivity
        {
            get
            {
                if (Servers[0]["logBasicSessionActivity"] != null)
                {
                    return bool.Parse(Servers[0]["logBasicSessionActivity"]);
                }
                return false;
            }
        }
        public static bool logAllSocketException
        {
            get
            {
                if (Servers[0]["logAllSocketException"] != null)
                {
                    return bool.Parse(Servers[0]["logAllSocketException"]);
                }
                return false;
            }
        }
        #endregion
    }
}
