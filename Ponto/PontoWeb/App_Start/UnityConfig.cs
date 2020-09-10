using PontoWeb.Utils;
using PontoWeb.Utils.Interface;
using System.Web.Mvc;
using Unity;
using Unity.Mvc5;

namespace PontoWeb
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();
            
           container.RegisterType<ISyncAsync, SyncAsync>();
           container.RegisterType<IEPaysConfig, EPaysConfig>();
            
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}