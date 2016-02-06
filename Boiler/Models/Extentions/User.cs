using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServiceStack.DataAnnotations;

namespace Boiler.Models
{
    public partial class User
    {
        [Ignore]
        public string Password { get; set; }
    }
}