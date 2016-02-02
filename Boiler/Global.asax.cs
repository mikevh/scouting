using System;
using System.IO;
using System.Web;

namespace Boiler
{
    public class Global : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e) {
            new AppHost().Init();
        }

        //protected void Application_BeginRequest(object sender, EventArgs e) {
        //    string url = Request.Url.LocalPath;
        //    if (!File.Exists(Context.Server.MapPath(url))) {
        //        Context.RewritePath("/index.html");
        //    }
        //}
    }
}