using BackService;
using FantansyNode.Common;
using FantasyNode.Entities;
using FantasyNode.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasyNode.Service
{
    public class BaseClientService : CommunicateBase, IClient
    {
        public static ClientUser CurrentUser=new ClientUser();
        public static string HostIp="0.0.0.0";

        public BaseClientService()
        {
            CurrentUser = new ClientUser();
        }

        public void SendNewestList()
        {

        }

        public void SendMessage(string msg)
        {

        }


        public void Register()
        {
            CommonOperation.RegistServiceCallBack(CurrentUser, HostIp, 9999, "BackService", CommunicateProtocolsEnum.TCP);
        }

        public void UnRegister()
        {

        }
    }
}
