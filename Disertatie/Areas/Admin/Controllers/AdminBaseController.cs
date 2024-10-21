using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Disertatie.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrator")]
    public abstract class AdminBaseController : Controller
    {
        
    }
}
