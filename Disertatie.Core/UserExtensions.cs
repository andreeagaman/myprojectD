using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Principal;
using Disertatie.Core.Models;

namespace Disertatie.Core
{
    public static class UserExtensions
    {
        public static bool EsteAdministrator(this IPrincipal user) {
            return user.IsInRole("Administrator");
        }

        public static bool EsteMembru(this IPrincipal user) {
            return user.IsInRole("Membru");
        }
        public static bool EsteSefDepartament(this IPrincipal user)
        {
            return user.IsInRole("Sef departament");
        }
        public static bool EsteCoordonatorColectiv(this IPrincipal user)
        {
            return user.IsInRole("Coordonator colectiv");
        }

    }
}
