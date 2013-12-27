using FantasyNode.Entities;
using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace FantasyNode.Service
{
    public class CommunicateBase:ICommonCommunicate
    {
        private FriendsCollection friends;

        public FriendsCollection Friends
        {
            get { return friends; }
            set { friends = value; }
        }
        /// <summary>
        /// 加入通讯录
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="zoo"></param>
        /// <param name="ip"></param>
        /// <param name="name"></param>
        public virtual void RegistMe(string guid, string zoo, string ip, string name)
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
        public virtual void UnRegisterMe(string guid)
        {
            if (string.IsNullOrEmpty(guid))
                throw new ArgumentNullException("guid不能为空，无法识别");
            FriendContext friend = this.Friends.Keys[guid];
            Friends.Remove(friend);
        }
    }
}
