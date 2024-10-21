using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Text;

namespace Disertatie.Helpers
{
    public static class HtmlMessageHelper
    {

        private static HtmlString MessageBoxInternal(this HtmlHelper html, string cssClass, string title, string details) {
            var sb = new StringBuilder("<div class=\"");
            sb.Append(cssClass);
            sb.Append("\"><span>");
            sb.Append(html.Encode(title));
            sb.Append("</span>");
            if (string.IsNullOrEmpty(details) == false) {
                sb.Append("<p>");
                sb.Append(html.Encode(details));
                sb.Append("</p>");
            }
            sb.Append("</div>");
            return new HtmlString(sb.ToString());
        }

        public static HtmlString ErrorBox(this HtmlHelper html, string title, string details) {
            return html.MessageBoxInternal("error", title, details);
        }

        public static HtmlString ErrorBox(this HtmlHelper html, string title) {
            return html.ErrorBox(title, null);
        }

        public static HtmlString SuccessBox(this HtmlHelper html, string title, string details) {
            return html.MessageBoxInternal("success", title, details);
        }

        public static HtmlString SuccessBox(this HtmlHelper html, string title) {
            return html.SuccessBox(title, null);
        }

        public static HtmlString WarningBox(this HtmlHelper html, string title, string details) {
            return html.MessageBoxInternal("notice", title, details);
        }

        public static HtmlString WarningBox(this HtmlHelper html, string title) {
            return html.WarningBox(title, null);
        }
    }
}