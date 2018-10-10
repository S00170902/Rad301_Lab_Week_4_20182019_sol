using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MVCClubsWeek4.Startup))]
namespace MVCClubsWeek4
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
