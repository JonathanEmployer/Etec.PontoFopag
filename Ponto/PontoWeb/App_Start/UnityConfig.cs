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

            container.RegisterType<IEPaysConfig, EPaysConfig>();
            container.RegisterType<ISyncAsync, SyncAsync>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}