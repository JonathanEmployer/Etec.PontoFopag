using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GerenciadorWeb.Startup))]
namespace GerenciadorWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
