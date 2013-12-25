/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:FantasyNode"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using FantansyNode.Common;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System.Configuration;

namespace FantasyNode.ViewModels
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// unity中间件,连通unity
        /// </summary>
        private UnityServiceLocator unityServiceLocator;
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            #region SimpleIOC
            //ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            
            ////if (ViewModelBase.IsInDesignModeStatic)
            ////{
            ////    // Create design time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DesignDataService>();
            ////}
            ////else
            ////{
            ////    // Create run time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DataService>();
            ////}
            //SimpleIoc.Default.Register<MainViewModel>();
            #endregion
            unityServiceLocator = (UnityServiceLocator)CreateServiceLocator();
            unityServiceLocator.GetInstance<LoginViewModel>();
        }

        public MainViewModel MainViewModel
        {
            get
            {
                return unityServiceLocator.GetInstance<MainViewModel>("MainViewModel");
            }
        }
        public LoginViewModel LoginViewModel
        {
            get
            {
                return unityServiceLocator.GetInstance<LoginViewModel>();
            }
        }
        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
        protected IServiceLocator CreateServiceLocator()
        {
            IUnityContainer container = new UnityContainer();
            UnityConfigurationSection section = (UnityConfigurationSection)ConfigurationManager.GetSection("unity");
            section.Containers.Default.Configure(container);
            return new UnityServiceLocator(container);
        }
    }
}