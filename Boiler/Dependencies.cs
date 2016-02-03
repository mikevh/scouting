using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Boiler.Repositories;
using Funq;

namespace Boiler
{
    public static class Dependencies
    {
        public static void RegisterApplicationDependencies(this Container container) {
            container.RegisterAutoWiredAs<TodoRepository, ITodoRepository>();
        }
    }
}