using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Helpers;
using System.Web.Routing;
using System.Web.Mvc.Html;
using System.Web.UI;
using System.IO;
using System.Linq.Expressions;
using System.Resources;
using System.Reflection;
using System.ComponentModel;
using Disertatie.Core;
using System.Net.Mail;
namespace Disertatie.Helpers
{
    public static class Utils
    {
        public static string EnumValue(Type objType, object objVal)
        {
            

            if (objType.IsEnum)
                if (Enum.IsDefined(objType, objVal))
                    Enum.GetName(objType, objVal);

            if (objVal != null)
                return objVal.ToString();

            else
                return string.Empty;
        }

        public static string Check(double lower, double upper, double toCheck)
        {
            return toCheck > lower && toCheck <= upper ? " checked=\"checked\"" : null;
        }

        public static string ToUrlString(this HttpServerUtilityBase server, string value) {
            return server.UrlEncode(value.ToLower().Replace(' ', '_').Replace(".", ""));
        }
     
        public static List<SelectListItem> CreateSelectItemList(object enumObj,
                                                                string defaultItemKey,
                                                                bool sortAlphabetically,
                                                                object firstValueOverride)
        {

           

            if (enumObj.GetType().IsEnum)
            {

                //ResourceHelpers.GetResourceValue(AppConstants.EnumResourceNamespace, enumObj.GetType().Name,
                //                    Enum.GetName(enumObj.GetType(),
                //                    currentItem)));

                var keyvaluePairs = Enum.GetValues(enumObj.GetType()).Cast<Int32>().ToDictionary(currentItem =>
                                                                                                    Utils.EnumValue(enumObj.GetType(),currentItem));

                return keyvaluePairs.ToSelectList(e => e.Key,
                                           e => e.Value.ToString(),
                                           !string.IsNullOrEmpty(defaultItemKey) ?  Utils.EnumValue(enumObj.GetType(),defaultItemKey) : string.Empty,
                                           enumObj,
                                           sortAlphabetically,
                                           firstValueOverride);
            }
            else
            {
                List<SelectListItem> list = new List<SelectListItem>();

                if (!string.IsNullOrEmpty(defaultItemKey))
                    list.Add(new SelectListItem() { Value = "-1", Text =  Utils.EnumValue(enumObj.GetType(),defaultItemKey) });

                return list;
            }
        }

        public static SelectList MakeSelection(this SelectList list, object selection)
        {
            return new SelectList(list.Items, list.DataValueField, list.DataTextField, selection);
        }
        public static ActionResult NotAuthorized(this ControllerBase controller) {
            return controller.NotAuthorized(message: null);
        }

        public static ActionResult NotAuthorized(this ControllerBase controller, string message) {
            
            if (message != null) {
                controller.TempData["Message"] = message;
            }

            controller.TempData["Url"] = controller.ControllerContext.HttpContext.Request.Url.ToString();

            return new RedirectToRouteResult(new RouteValueDictionary(new {
                controller = "Error",
                action = "NotAuthorized"
            }));
        }

        public static void SendEmail(MailAddress fromAddress, MailAddress toAddress, string subject, string body)
        {

          var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            };

            var client = new SmtpClient("mail.astrasig.ro");
            try
            {
                client.SendAsync(message, null);
            }
            catch (Exception) { }
         
        }
    }
}