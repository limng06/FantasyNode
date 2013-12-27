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
    public class BackService : CommunicateBase, IBackService
    {
        public BackService()
        {
            Friends = new FriendsCollection();
        }
        /// <summary>
        /// 
        /// </summary>
        public void ImNew()
        {

        }




        public void Register(string guid, string zoo, string ip, string name)
        {
            RegistMe(guid, zoo, ip, name);
        }

        public void UnRegister(string guid)
        {
            
        }
    }
}
