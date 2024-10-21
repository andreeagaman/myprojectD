using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Disertatie.Mvc
{
    public class HttpCreated : ActionResult
    {
        public override void ExecuteResult(ControllerContext context) {
            context.HttpContext.Response.StatusCode = 201;
        }
    }
}
