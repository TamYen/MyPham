using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MyPham.Startup))]
namespace MyPham
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
