using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Disertatie.Core.Models;
using Disertatie.Core.Repositories;

namespace Disertatie.Core
{
    public class SecurityContext
    {
        public static Utilizator CurrentUser { 
            get {
                var ctx = HttpContext.Current;
                if (ctx != null) {
                    if (ctx.User.Identity.IsAuthenticated) {
                        var user = ctx.Items["__User"] as Utilizator;
                        if (user == null) {
                            user = new UtilizatorRepository().GetUserByName(ctx.User.Identity.Name, null);
                            ctx.Items["__User"] = user;                            
                        }
                        return user;
                    }
                }
                return null;
            }
        }
    }
}
