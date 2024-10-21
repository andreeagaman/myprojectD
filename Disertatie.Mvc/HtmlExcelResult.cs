using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.UI;

namespace Disertatie.Mvc
{
    public class HtmlExcelResult : ActionResult
    {
        private readonly object _data;
        public HtmlExcelResult(object data)
        {
            _data = data;
        }

        public object Data { get { return _data; } }

        public override void ExecuteResult(ControllerContext context)
        {
            var grid = new System.Web.UI.WebControls.GridView();
            grid.DataSource = _data;
            grid.DataBind();

            context.HttpContext.Response.ClearHeaders();
            context.HttpContext.Response.AddHeader("content-disposition", "attachment; filename=Statistica.xls");
            context.HttpContext.Response.ContentType = "application/excel";

            var htmlWriter = new HtmlTextWriter(context.HttpContext.Response.Output);
            grid.RenderControl(htmlWriter);
        }
    }
}
