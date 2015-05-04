using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PartyUp.Startup))]
namespace PartyUp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
