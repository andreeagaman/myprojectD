using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Disertatie.Models
{
    public class Regexes
    {
        public const string Username = @"^[-\.a-zA-Z0-9_]{5,20}$";
        public const string Email = @"^\s*([_a-z0-9-]+(\.[_a-z0-9-]+)*@[a-z0-9-]+(\.[a-z0-9-]+)*(\.[a-z]{2,4}))\s*$";
    }
}