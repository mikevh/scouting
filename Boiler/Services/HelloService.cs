using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServiceStack;

namespace Boiler.Models
{
    public class HelloService : IService
    {
        public object Any(HelloRequest request)
        {
            var name = request.Name ?? "FOo Bar";

            return new HelloResponse
            {
                Result = "Hello, " + name
            };
        }
    }

    [Route("/hello")]
    [Route("/hello/{name}")]
    public class HelloRequest
    {
        public string Name { get; set; }
    }

    public class HelloResponse
    {
        public string Result { get; set; }
    }
}