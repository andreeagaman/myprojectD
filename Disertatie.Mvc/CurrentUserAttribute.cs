using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Disertatie.Core;
using Disertatie.Core.Models;
using Disertatie.Core.Repositories;

namespace Disertatie.Mvc
{
    public class CurrentUserAttribute : ActionFilterAttribute
    {
        public const string CurrentUserKey = "__CurrentUser";

        public CurrentUserAttribute() {
            this.Order = 20;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext) {            
            var userIdentity = filterContext.HttpContext.User.Identity;
            if (userIdentity.IsAuthenticated) {
                if (filterContext.IsChildAction == false) {
                    filterContext.HttpContext.Items[CurrentUserKey] = CreateLazyUser(userIdentity.Name);
                }
            }
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext) {
            var lazyUser = filterContext.HttpContext.Items[CurrentUserAttribute.CurrentUserKey] as Lazy<Utilizator>;
            if (lazyUser != null) {
                filterContext.Controller.ViewBag.User = lazyUser.Value;
            }
        }

        private Lazy<Utilizator> CreateLazyUser(string username) {
            return new Lazy<Utilizator>(() =>
            {
                return new UtilizatorRepository().GetUserByName(username, null);
            }, true);
        }
    }
}
