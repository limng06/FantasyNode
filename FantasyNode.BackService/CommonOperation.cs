using FantansyNode.Common;
using FantasyNode.Entities;
using FantasyNode.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BackService
{
    public static class CommonOperation
    {
        /// <summary>
        /// 客户端使用 想服务端注册后 回调服务端的注册服务
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="serviceName"></param>
        /// <param name="protocolType"></param>
        public static void RegistService(ClientUser cUser, string ip, int port, string serviceName, CommunicateProtocolsEnum protocolType)
        {
            Thread.Sleep(1500);
            string serviceURL = MakeServiceString(ip, port, serviceName, protocolType);
            EndpointAddress address = new EndpointAddress(serviceURL);
            try
            {
                System.ServiceModel.Channels.Binding bindingInstance = null;
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
                    channelClient.Register(cUser.Guid.ToString(), cUser.Zoo, ip, cUser.Name);
                    client.Register();
                    Console.WriteLine(serviceURL + "  :Success!");
                }
            }
            catch (Exception e)
            {
                //log4net.LogManager.GetLogger("FantasyNode.Logging").Debug(serviceURL.ToString() + "没有回应");
                Console.WriteLine(serviceURL + "  :fail!    " + e.Message);
            }
        }

        /// <summary>
        /// 客户端使用 xiang服务端注册
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="serviceName"></param>
        /// <param name="protocolType"></param>
        public static void RegistServiceCallBack(ClientUser cUser, string ip, int port, string serviceName, CommunicateProtocolsEnum protocolType)
        {
            string serviceURL = MakeServiceString(ip, port, serviceName, protocolType);
            EndpointAddress address = new EndpointAddress(serviceURL);
            try
            {
                System.ServiceModel.Channels.Binding bindingInstance = null;
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
                    channelClient.Register(cUser.Guid.ToString(), cUser.Zoo, ip, cUser.Name);
                    Console.WriteLine(serviceURL + "  :Success!");
                }
            }
            catch (Exception e)
            {
                //log4net.LogManager.GetLogger("FantasyNode.Logging").Debug(serviceURL.ToString() + "没有回应");
                Console.WriteLine(serviceURL + "  :fail!    " + e.Message);
            }
        }

        /// <summary>
        /// 组合字符串
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="serviceName"></param>
        /// <param name="protocolType"></param>
        /// <returns></returns>
        public static string MakeServiceString(string ip, int port, string serviceName, CommunicateProtocolsEnum protocolType)
        {
            StringBuilder sbService = new StringBuilder();
            switch (protocolType)
            {
                case CommunicateProtocolsEnum.HTTP: sbService.Append("http"); break;
                case CommunicateProtocolsEnum.TCP: sbService.Append("net.tcp"); break;
                default: sbService.Append("http"); break;
            }
            return sbService.Append("://").Append(ip).Append(port == 0 ? "" : (":" + port.ToString())).Append("/").Append(serviceName).ToString(); ;
        }
    }
}
