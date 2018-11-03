using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(VehicleRouting.Startup))]

namespace VehicleRouting
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            this.ConfigureAuth(app);
        }
    }
}
