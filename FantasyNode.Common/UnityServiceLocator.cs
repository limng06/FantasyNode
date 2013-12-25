using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;

namespace FantansyNode.Common
{
    public class UnityServiceLocator : ServiceLocatorImplBase
    {
        private IUnityContainer container;

        public UnityServiceLocator(IUnityContainer _container)
        {
            this.container = _container;
        }

        protected override object DoGetInstance(Type serviceType, string key)
        {
            return container.Resolve(serviceType, key);
        }

        protected override IEnumerable<object> DoGetAllInstances(Type serviceType)
        {
            return container.ResolveAll(serviceType);
        }
    }
}
