using Microsoft.Owin;
using Owin;

// Attributes are always applied to an element (e.g. a method, property)
// The "assembly:" prefix means that the attribute is applied to the assembly.
[assembly: OwinStartup(typeof(ExpenseTracker.API.Startup))]

namespace ExpenseTracker.API
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseWebApi(WebApiConfig.Register());
        }
    }
}