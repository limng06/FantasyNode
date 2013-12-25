using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace FantasyNode.Interfaces
{
    [ServiceContract]
    public interface IClient
    {
        /// <summary>
        /// 发送最新的客户端列表
        /// </summary>
        [OperationContract(IsOneWay = true)]
        void SendNewestList();
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="msg"></param>
        [OperationContract(IsOneWay = true)]
        void SendMessage(string msg);
    }
}
