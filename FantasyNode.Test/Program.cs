using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Net.Sockets;
using System.Net;
using FantasyNode.Interfaces;
using System.Reflection;
using FantasyNode.Service;

namespace TestConnection
{
    class Program
    {
        public static IPHostEntry myHost = Dns.GetHostEntry(Dns.GetHostName());
        public static string hostIp = myHost.AddressList.First(t => t.AddressFamily == AddressFamily.InterNetwork).ToString();
        public static object object1;
        static void Main(string[] args)
        {


            ///测试，从192.168.1.1-192.168.1.254开始查找
            Action<int> DoSearchService = new Action<int>(t => { FindServiceOnIP(t); });
            System.Threading.Tasks.Parallel.For(100, 200, DoSearchService);
            Console.ReadKey();
        }

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
                    Console.WriteLine(serviceURL + "  :Success!");
                }
            }
            catch (Exception e)
            {
                log4net.LogManager.GetLogger("FantasyNode.Logging").Debug(serviceURL.ToString() + "没有回应");
                Console.WriteLine(serviceURL + "  :fail!    " + e.Message);
            }
        }
    }
}
