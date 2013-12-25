using FantasyNode.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace FantasyNode.ViewModels
{
    public  class MainViewModel
    {
        public static IPHostEntry myHost = Dns.GetHostEntry(Dns.GetHostName());
        public static string hostIp = myHost.AddressList.First(t => t.AddressFamily == AddressFamily.InterNetwork).ToString();
        public static object object1;
        public static log4net.ILog log = App.log;
        /// <summary>
        /// 方法：查找局域网计算机
        /// </summary>
        public static void InvokeFindFriends()
        {
            Action<int> DoSearchService = new Action<int>(t => { FindServiceOnIP(t); });
            System.Threading.Tasks.Parallel.For(100, 200, DoSearchService);
        }
        /// <summary>
        /// 根据ip查找服务
        /// </summary>
        /// <param name="i"></param>
        public static void FindServiceOnIP(int i)
        {
            string serviceURL = "net.tcp://" + "192.168.1." + i.ToString() + ":9999/BackService";
            try
            {
                EndpointAddress address = new EndpointAddress(serviceURL);
                Binding bindingInstance = null;
                NetTcpBinding tcpBinding = new NetTcpBinding();
                tcpBinding.MaxReceivedMessageSize = 20971520;
                tcpBinding.ReceiveTimeout = new TimeSpan(100000000);
                tcpBinding.SendTimeout = new TimeSpan(100000000);
                tcpBinding.Security.Mode = SecurityMode.None;
                bindingInstance = tcpBinding;
                var client = new FantasyNode.Service.BaseClientService();
                var instanceContext = new InstanceContext(client);
                using (DuplexChannelFactory<IBackService> channel = new DuplexChannelFactory<IBackService>(instanceContext, bindingInstance, address))
                {
                    //channel.Faulted+=  发生错误的时候处理 记录行为
                    var channelClient = channel.CreateChannel();
                    channelClient.GetType();

                    channelClient.Register(Guid.NewGuid().ToString(), "SHB", hostIp, "liming");
                    log.Info(serviceURL + "  :Success!");
                }
            }
            catch (Exception e)
            {
                log4net.LogManager.GetLogger("FantasyNode.Logging").Debug(serviceURL.ToString() + "没有回应");
                log.Debug(serviceURL + "  :fail!    " + e.Message);
            }
        }
    }
}
