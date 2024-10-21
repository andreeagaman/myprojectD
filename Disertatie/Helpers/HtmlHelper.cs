using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Helpers;
using System.Web.Mvc.Html;

using System.Web.UI;
using System.IO;
using System.Linq.Expressions;
using System.Resources;


using System.Text;
using Disertatie.Core;


namespace Disertatie.Helpers
{
    public static class HtmlElementHelper
    {

        
        public static MvcHtmlString EnumDropDownList(this HtmlHelper htmlHelper,
                                                            object enumObj,
                                                            string name,
                                                            string defaultItemKey,
                                                            bool sortAlphabetically,
                                                            object firstValueOverride,
                                                            Dictionary<string,string> dict)
        //where TEnum : struct
        {

    

            StringBuilder sbRezultat = new StringBuilder();

            //var options = Utils.CreateSelectItemList(enumObj, defaultItemKey, sortAlphabetically, firstValueOverride);

          
         

             int selectedIndex=0;
          
             var selectItemList = new List<SelectListItem>();


            if (enumObj != null)
            {
                selectItemList = Disertatie.Helpers.Utils.CreateSelectItemList(enumObj, defaultItemKey, true, null);
                
                var selectedItem = selectItemList.SingleOrDefault(item => item.Selected);
                if (selectedItem != null)
                {
                    selectedIndex = selectItemList.IndexOf(selectedItem);
                  
                }
            }
            sbRezultat.Append("<select ");
            string id = string.Empty;
            dict.TryGetValue("id", out id);

            if(!string.IsNullOrEmpty(id))
                sbRezultat.Append("id='"+id+"' ");
            string class1 = string.Empty;
            dict.TryGetValue("class", out class1);
            if(!string.IsNullOrEmpty(class1))
            sbRezultat.Append("class='("+class1+"!=null? "+class1+":string.Empty)'");

            //string name1 = string.Empty;
            //dict.TryGetValue("name", out name1);
            if (!string.IsNullOrEmpty(name))
                sbRezultat.Append("name='" + name + "'");
            string onchange = string.Empty;
            dict.TryGetValue("onchange", out onchange);
            if (!string.IsNullOrEmpty(onchange))
                if (onchange == "readonly")
                {
                    sbRezultat.Append("onchange='this.selectedIndex="+selectedIndex+  ";'");
                    sbRezultat.Append(" style='background: lightgrey';"); 
                }
                   
            sbRezultat.Append(">");
            foreach (var option in selectItemList)
            {
                sbRezultat.Append(" <option value='");
                sbRezultat.Append(option.Value);
                sbRezultat.Append("' ");
                if(option.Selected)
                    sbRezultat.Append("selected");
                sbRezultat.Append(" >");
                sbRezultat.Append(option.Text);
                
                
                sbRezultat.Append("</option>");
            }
            sbRezultat.Append("</select>");
            return new MvcHtmlString(sbRezultat.ToString());
        }

    }
}