using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using GalaSoft.MvvmLight.Messaging;
using FantasyNode.ViewModels;

namespace FantasyNode.Views
{
    /// <summary>
    /// Login.xaml 的交互逻辑
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
            Messenger.Default.Register<NotificationMessage>(this, "LoginSuccessMessage", LoginSuccessNavigation);
        }
        /// <summary>
        /// 登陆成功，转向主页面
        /// </summary>
        /// <param name="msg"></param>
        private void LoginSuccessNavigation(NotificationMessage msg)
        {
            Messenger.Default.Send<NotificationMessage>(new NotificationMessage("/MainWindow.xaml"), "MainWindowNavigation");
            this.Close();
        }
    }
}
