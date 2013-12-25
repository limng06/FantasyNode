using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FantasyNode.Entities;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Windows.Input;
using GalaSoft.MvvmLight.Messaging;

namespace FantasyNode.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {

        #region private
        private ClientUser _cUser;
        private bool? isRemembered;
        #endregion

        #region Public
        /// <summary>
        /// IsRemembered
        /// </summary>
        public bool? IsRemembered
        {
            get { return isRemembered; }
            set
            {
                if (value != isRemembered)
                {
                    isRemembered = value;
                    RaisePropertyChanged(() => this.Name);
                }
            }
        }
        /// <summary>
        /// Name
        /// </summary>
        public string Name
        {
            get { return _cUser.Name; }
            set
            {
                if (value != Name)
                {
                    _cUser.Name = value;
                    RaisePropertyChanged(() => this.Name);
                }
            }
        }
        /// <summary>
        /// Zoo
        /// </summary>
        public string Zoo
        {
            get { return _cUser.Zoo; }
            set
            {
                if (value != Zoo)
                {
                    _cUser.Zoo = value;
                    RaisePropertyChanged(() => this.Name);
                }
            }
        }
        /// <summary>
        /// 当前用户
        /// </summary>
        public ClientUser CurrentUser
        {
            get { return _cUser; }
            set
            {
                if (CurrentUser != value)
                {
                    _cUser = value;
                    RaisePropertyChanged(() => this.CurrentUser);
                }
            }
        }
        ///////////////////////////////////
        public ICommand LoginCmd { get; protected set; }
        public ICommand ExitCmd { get; protected set; }

        #endregion

        #region Commands实现
        public void ExcuteLogin()
        {
            ///传递CurrentUser
            this._cUser.Guid = App.MyGuid;
            App.CurrentUser = this.CurrentUser;
            //启动后台服务
            App.StartBackService();
            //开始查找服务 
            App.InvokeFindFriends();
            //发送跳转页面的message
            Messenger.Default.Send<NotificationMessage>(new NotificationMessage(""), "LoginSuccessMessage");
        }

        #endregion
        #region 构造函数
        public LoginViewModel()
        {
            this._cUser = new ClientUser();
            this.LoginCmd = new RelayCommand(() => ExcuteLogin());
        }
        #endregion

    }
}
