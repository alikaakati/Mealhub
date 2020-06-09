using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MealHubProject.Startup))]
namespace MealHubProject
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
