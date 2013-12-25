using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace FantasyNode.Entities
{
    [Serializable]
    public class FriendContext
    {
        private string guid;
        /// <summary>
        /// 唯一标示
        /// </summary>
        public string Guid
        {
            get { return guid; }
            set { guid = value; }
        }

        private string name;
        /// <summary>
        /// 姓名标示
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private string zoo;
        /// <summary>
        /// 动物园（你在哪个动物园）
        /// </summary>
        public string Zoo
        {
            get { return zoo; }
            set { zoo = value; }
        }
        private string ip;
        /// <summary>
        /// ip address
        /// </summary>
        public string IP
        {
            get { return ip; }
            set { ip = value; }
        }


        private OperationContext operationContext;
        /// <summary>
        /// client的上下文
        /// </summary>
        public OperationContext OperationContext
        {
            get { return operationContext; }
            set { operationContext = value; }
        }
        public FriendContext(string _guid,string _zoo, string _name, string _ip, OperationContext _operationContext)
        {
            this.Guid = _guid;
            this.Zoo = _zoo;
            this.Name = _name;
            this.OperationContext = _operationContext;
            this.ip = _ip;
        }
    }
}
