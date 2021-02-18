using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SecondHandAuth.Startup))]
namespace SecondHandAuth
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
