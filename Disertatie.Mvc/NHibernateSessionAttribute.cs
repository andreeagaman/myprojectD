using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using NHibernate;
using Disertatie.Core;

namespace Disertatie.Mvc
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class NHibernateSessionAttribute : ActionFilterAttribute
    {
        public bool RollbackOnModelStateError { get; set; }
        public ISession Session { get; private set; }
        public ITransaction Transaction { get; private set; }

        public NHibernateSessionAttribute() {
            this.Order = 10;
            this.RollbackOnModelStateError = false;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext) {
            // this code also performs a check if the session is already bound, 
            // ... and if not it binds it to the CurrentSessionContext
            this.Session = SessionManager.GetCurrentSession();
            this.Transaction = this.Session.BeginTransaction();

            base.OnActionExecuting(filterContext);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext) {
            
            base.OnActionExecuted(filterContext);

            var forceTransactionRollback = ShouldRollback(filterContext) || UnhandledExeption(filterContext);
            SessionManager.UnbindSession(forceTransactionRollback);
        }

        private bool ShouldRollback(ControllerContext context) {
            return RollbackOnModelStateError && !context.Controller.ViewData.ModelState.IsValid;
        }

        private bool UnhandledExeption(ActionExecutedContext context) {
            return context.Exception != null && context.ExceptionHandled == false;
        }
    }
}
