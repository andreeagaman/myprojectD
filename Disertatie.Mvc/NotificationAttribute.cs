using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Disertatie.Core;
using NHibernate;
using Disertatie.Core.Repositories;

namespace Disertatie.Mvc
{
    public class NotificationAttribute : ActionFilterAttribute
    {
        private readonly string[] layouts;

        public NotificationAttribute(params string[] layouts) {
            if (layouts == null || layouts.Length == 0)
                throw new ArgumentException("layouts");

            this.layouts = layouts;
            Order = 30;
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext) {
            if (this.ShouldUpdateNewAssignmentsCount(filterContext)) {
                var user = filterContext.HttpContext.User;
                if (user.EsteMembru()|| user.EsteCoordonatorColectiv() || user.EsteSefDepartament())
                {
                    bool wasAlreadyBoundToContext;
                    var session = SessionManager.GetCurrentSession(out wasAlreadyBoundToContext);
                    try
                    {

                        if (!wasAlreadyBoundToContext)
                            session.BeginTransaction();

                        var r = new UtilizatorRepository(session);
                        var count = r.GetNrMesajeNoiPtUtilizator(user.Identity.Name);
                        filterContext.Controller.ViewBag.NrMesajePtUtilizator =count;

                        if (!wasAlreadyBoundToContext)
                            session.Transaction.Commit();
                    }
                    finally
                    {
                        if (wasAlreadyBoundToContext == false)
                            session.Dispose();
                    }
                }                
            }
            
        }

        public override void OnResultExecuting(ResultExecutingContext filterContext) {
            if (filterContext.Result is ViewResult) {
                   
            }
        }

        private bool ShouldUpdateNewAssignmentsCount(ActionExecutedContext filterContext) {
            
            if (filterContext.Canceled)
                return false;

            if (filterContext.Exception != null)
                return false;

            if (filterContext.Result is ViewResult) {
                return true;
                //var view = (ViewResult)filterContext.Result;
                //return layouts.Contains(view.MasterName, StringComparer.OrdinalIgnoreCase);                
            }

            return false;
        }
    }
}
