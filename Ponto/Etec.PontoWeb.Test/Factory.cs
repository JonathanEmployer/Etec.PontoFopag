using Autofac;
using Autofac.Builder;
using PontoWeb.Utils;
using PontoWeb.Utils.Interface;

namespace Etec.PontoWeb.Test
{
    public class IoCModule : Module
    {
        protected override void Load(Autofac.ContainerBuilder builder)
        {
            builder.RegisterType<SyncAsync>().As<ISyncAsync>();
            builder.RegisterType<EPaysConfig>().As<IEPaysConfig>();
        }
    }
    public class Factory
    {
        private IContainer container;
        public Factory()
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule<IoCModule>();
            this.container = containerBuilder.Build(
                ContainerBuildOptions.IgnoreStartableComponents);
        }

        public IContainer GetContainer()
        {
            return container;
        }
    }
}
