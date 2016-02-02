using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServiceStack;
using ServiceStack.FluentValidation;

namespace Boiler.Models
{
    public class HelloService : SecureBaseService
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
    public class HelloRequest : IReturn<HelloResponse>
    {
        public string Name { get; set; }
    }

    public class HelloResponse
    {
        public string Result { get; set; }
    }

    public class HelloRequestValidator : AbstractValidator<HelloRequest>
    {
        public HelloRequestValidator() {
            RuleFor(x => x.Name).Length(5, Int32.MaxValue).When(x => !string.IsNullOrEmpty(x.Name));
        }
    }
}