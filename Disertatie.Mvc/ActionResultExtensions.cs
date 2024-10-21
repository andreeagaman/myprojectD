using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Disertatie.Mvc
{
    public static class ActionResultExtensions
    {
        public static ActionResult HttpCreated(this Controller controller) {
            return new HttpCreated();
        }
    }
}
