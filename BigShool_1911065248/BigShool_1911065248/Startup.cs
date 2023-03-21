using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BigShool_1911065248.Startup))]
namespace BigShool_1911065248
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
