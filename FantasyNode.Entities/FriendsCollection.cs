using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using FantasyNode.Entities;

namespace FantasyNode.Entities
{
    public class FriendsCollection : ObservableCollection<FriendContext>
    {
        private Dictionary<string, FriendContext> keys;
        public Dictionary<string, FriendContext> Keys
        {
            get { return keys; }
            set { keys = value; }
        }

        public FriendsCollection() {
            keys = new Dictionary<string, FriendContext>();
        }
        /// <summary>
        /// 检查是否存在相同的项
        /// </summary>
        /// <param name="friend"></param>
        /// <returns></returns>
        public bool Contains(FriendContext friend)
        {
            bool res = false;
            if (keys.ContainsKey(friend.Guid))
                return true;
            else
                return res;
        }
        /// <summary>
        /// 重载InsertItem函数，添加对Keys的支持
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        protected override void InsertItem(int index, FriendContext item)
        {
            try
            {
                string guid = item.Guid;
                base.InsertItem(index, item);
                Keys.Add(guid, item);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 重载removeItem函数
        /// </summary>
        /// <param name="index"></param>
        protected override void RemoveItem(int index)
        {
            try
            {
                string guid = Items[index].Guid;
                base.RemoveItem(index);
                Keys.Remove(guid);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
