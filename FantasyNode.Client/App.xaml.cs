using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.ServiceModel;
using FantasyNode.Entities;
using FantasyNode.Interfaces;
using System.Net;
using FantasyNode.Service;
using System.Net.Sockets;
using System.ServiceModel.Channels;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.ComponentModel;
using GalaSoft.MvvmLight.Messaging;
using FantasyNode.ViewModels;
using Microsoft.Practices.Unity;

namespace FantasyNode
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        public static ServiceHost Host;
        public static ClientUser CurrentUser;
        public static Guid MyGuid = Guid.NewGuid();
        public static BackgroundWorker FindFriendsWorker = new BackgroundWorker();
        public static log4net.ILog log;//
        public static IPHostEntry myHost = Dns.GetHostEntry(Dns.GetHostName());
        public static string hostIp = myHost.AddressList.First(t => t.AddressFamily == AddressFamily.InterNetwork).ToString();
        public static object object1;
        /// <summary>
        /// 静态构造函数
        /// </summary>
        static App()
        {
            log = log4net.LogManager.GetLogger("FantasyNode.Logging");
            log.Info("Start App");
        }
        public App()
        {
            App.Current.Startup += this.App_Started;
        }
        /// <summary>
        /// 启动后台服务
        /// </summary>
        /// <returns></returns>
        public static bool StartBackService()
        {
            Host = new ServiceHost(typeof(FantasyNode.Service.BackService));
            //IPHostEntry myHost = Dns.GetHostEntry(Dns.GetHostName());
            //string hostIp = myHost.AddressList.First(t => t.AddressFamily == AddressFamily.InterNetwork).ToString();
            //Host.AddServiceEndpoint(typeof(IBackService),new NetTcpBinding(),new Uri("net.tcp://"+hostIp.ToString()+"/BackService"));
            Host.Open();
            log.Info("StartBackService");
            return true;
        }
        /// <summary>
        /// 方法：查找局域网计算机
        /// </summary>
        public static void InvokeFindFriends()
        {
            Action<int> DoSearchService = new Action<int>(t => { FindServiceOnIP(t); });
            Thread searchThread = new Thread(new ThreadStart(() =>
            {
                System.Threading.Tasks.Parallel.For(100, 200, new ParallelOptions(), DoSearchService);
            }));
        }
        public static ProgressChangedEventHandler ReportProgress;
        public static RunWorkerCompletedEventHandler FindFriendsCompleted;
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
        /// <summary>
        /// 查找局域网中计算机，比较耗时，没有办法==============弃用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void FindFriends(object sender, DoWorkEventArgs e)
        {
            if (Host.State == CommunicationState.Opened)
            {
                IPHostEntry myHost = Dns.GetHostEntry(Dns.GetHostName());
                string hostIp = myHost.AddressList.First(t => t.AddressFamily == AddressFamily.InterNetwork).ToString();

                string serviceURL = string.Empty;
                ///测试，从192.168.1.1-192.168.1.254开始查找
                Stopwatch watch = new Stopwatch();
                for (int i = 1; i < 254; i++)
                {
                    try
                    {
                        serviceURL = "net.tcp://" + "192.168.1." + i.ToString() + " + :9999/BackService";
                        EndpointAddress address = new EndpointAddress(serviceURL);
                        Binding bindingInstance = null;
                        NetTcpBinding tcpBinding = new NetTcpBinding();
                        tcpBinding.MaxReceivedMessageSize = 20971520;
                        tcpBinding.ReceiveTimeout = new TimeSpan(1000);
                        tcpBinding.SendTimeout = new TimeSpan(1000);
                        tcpBinding.Security.Mode = SecurityMode.None;
                        bindingInstance = tcpBinding;
                        var client = new BaseClientService();
                        var instanceContext = new InstanceContext(client);
                        using (DuplexChannelFactory<IBackService> channel = new DuplexChannelFactory<IBackService>(instanceContext, bindingInstance, address))
                        {
                            //channel.Faulted+=  发生错误的时候处理 记录行为
                            var channelClient = channel.CreateChannel();
                            channelClient.Register(MyGuid.ToString(), "SHB", hostIp, "liming");
                        }
                    }
                    catch (Exception)
                    {
                        log4net.LogManager.GetLogger("FantasyNode.Logging").Debug(serviceURL.ToString() + "没有回应");
                    }
                }
            }
        }
        /// <summary>
        /// 跳转主页面
        /// </summary>
        /// <param name="msg"></param>
        private void MainWindowNavigation(NotificationMessage msg)
        {
            Views.Main mainView = new Views.Main();
            mainView.Show();
        }
        public void App_Started(object sender, StartupEventArgs e)
        {
            //只允许一个实例运行
            string strProgName = System.Diagnostics.Process.GetCurrentProcess().ProcessName.ToUpper();
            System.Diagnostics.Process[] procList = System.Diagnostics.Process.GetProcessesByName(strProgName);
            if (procList.Length > 1)
            {
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
            ///注册消息
            Messenger.Default.Register<NotificationMessage>(this, "MainWindowNavigation", this.MainWindowNavigation);
        }
    }
}
