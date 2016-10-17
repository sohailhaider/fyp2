using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Web_omlate.ChatModule.Startup))]

namespace Web_omlate.ChatModule
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}