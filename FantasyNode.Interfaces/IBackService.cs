using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using FantasyNode.Entities;

namespace FantasyNode.Interfaces
{
    [ServiceContract(CallbackContract = typeof(IClient))]
    public interface IBackService
    {
        /// <summary>
        /// 广播我是最新的,听到的人对比自己的时间戳 ，如果比自己新，那自己就关闭广播
        /// </summary>
        [OperationContract]
        void ImNew();
        /// <summary>
        /// 注册客户端
        /// </summary>
        [OperationContract]
        void Register(string guid, string zoo, string ip,string name);
        /// <summary>
        /// 卸载客户端
        /// </summary>
        [OperationContract]
        void UnRegister(string guid);
    }
}
