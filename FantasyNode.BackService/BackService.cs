using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FantasyNode.Interfaces;
using System.ServiceModel;
using FantasyNode.Extends;
using FantasyNode.Entities;

namespace FantasyNode.Service
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class BackService : IBackService
    {
        private FriendsCollection friends;

        public FriendsCollection Friends
        {
            get { return friends; }
            set { friends = value; }
        }

        public BackService()
        {
            Friends = new FriendsCollection();
        }

        public void ImNew()
        {

        }

        public void Register(string guid, string zoo, string ip, string name)
        {
            if (string.IsNullOrEmpty(guid) || string.IsNullOrEmpty(zoo) || string.IsNullOrEmpty(name))
                throw new ArgumentNullException("域名和用户名不可以为空！");
            OperationContext opContext = OperationContext.Current;
            if (opContext == null)
                throw new ArgumentNullException("未能获取到当前的OperationContext");
            FriendContext friend = new FriendContext(guid, zoo, name, ip, opContext);
            // 是否重复注册，重复注册则覆盖或者取消操作
            Friends.Add(friend);

            //允许弹出上线提示

        }
        /// <summary>
        /// 从通讯录中删除
        /// </summary>
        /// <param name="guid"></param>
        public void UnRegister(string guid)
        {
            if (string.IsNullOrEmpty(guid))
                throw new ArgumentNullException("guid不能为空，无法识别");
            FriendContext friend = this.Friends.Keys[guid];
            Friends.Remove(friend);
        }
    }
}
