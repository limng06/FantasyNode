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
using BackService;
using FantansyNode.Common;

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
            Host.Open();
            log.Info("StartBackService");
            Console.WriteLine("StartService");
            return true;
        }
        /// <summary>
        /// 方法：查找局域网计算机
        /// </summary>
        public static void InvokeFindFriends()
        {
            Action<int> DoSearchService = new Action<int>(t =>
            {
                string ip = "192.168.254." + t.ToString();
                CommonOperation.RegistService(CurrentUser, ip, 9999, "BackService", CommunicateProtocolsEnum.TCP);
            });
            Thread searchThread = new Thread(new ThreadStart(() =>
            {
                ParallelOptions parallelOptions = new ParallelOptions();
                parallelOptions.MaxDegreeOfParallelism = 2;
                System.Threading.Tasks.Parallel.For(5, 20, parallelOptions, DoSearchService);
            }));
            searchThread.Start();
        }
        public static ProgressChangedEventHandler ReportProgress;
        public static RunWorkerCompletedEventHandler FindFriendsCompleted;

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
