using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasyNode.Entities
{
    public class ClientUser
    {
        private Guid guid;
        /// <summary>
        /// 唯一标示
        /// </summary>
        public Guid Guid
        {
            get { return guid; }
            set { guid = value; }
        }
        private string zoo;
        /// <summary>
        /// 域名
        /// </summary>
        public string Zoo
        {
            get { return zoo; }
            set { zoo = value; }
        }
        private string name;
        /// <summary>
        /// 称呼
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public ClientUser()
        { }
        public ClientUser(Guid _guid, string _zoo, string _name)
        {
            this.Guid = _guid;
            this.Zoo = _zoo;
            this.Name = _name;
        }

    }
}
