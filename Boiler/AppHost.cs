using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Boiler.Models;
using Funq;
using ServiceStack;

namespace Boiler
{
    public class AppHost : AppHostBase
    {
        public AppHost() : base("Hello", typeof(HelloService).Assembly)
        {
            
        }

        public override void Configure(Container container)
        {

        }
    }
}