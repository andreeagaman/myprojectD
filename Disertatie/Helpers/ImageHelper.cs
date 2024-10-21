using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace Disertatie.Helpers
{
    public static class ImageHelper
    {
        private const string imgTag = "<img src=\"{0}\" alt=\"{1}\" />";

        public static IHtmlString Thumbnail(this HtmlHelper helper, int width, int height, string file) {
            return helper.Thumbnail(width, height, file, null);
        }

        public static IHtmlString Thumbnail(this HtmlHelper helper, int width, int height, string file, string alt) {
            return helper.Thumbnail(width, height, file, new { alt });
        }

        public static IHtmlString Thumbnail(this HtmlHelper helper, int width, int height, string file, object htmlAttributes) {
            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);
            var tagBuilder = new TagBuilder("img");
            
            var url = urlHelper.Action("thumbnail", "images", new { width, height, src = file });
            tagBuilder.MergeAttribute("src", url);
            
            //tagBuilder.MergeAttribute("src", urlHelper.Content(string.Format("~/images.aspx/thumbnail?width={0}&height={1}&src={2}", width, height, file)));
            
            if (htmlAttributes != null) {
                tagBuilder.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            }
            return new HtmlString(tagBuilder.ToString(TagRenderMode.SelfClosing));
        }

        public static string Thumbnail(this UrlHelper helper, int width, int height, string file) {
            return helper.Action("thumbnail", "images", new { width, height, src = file });
            //return helper.Content(string.Format("~/images/thumbnail?width={0}&height={1}&src={2}", width, height, file));
        }
    }
}