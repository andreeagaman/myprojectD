using System.Web.Mvc;
using System.Collections.Generic;
using System;
using System.Linq;

public static class IEnumerableHelpers
{
    public static List<SelectListItem> ToSelectList<T>(
        this IEnumerable<T> enumerable,
        Func<T, string> text,
        Func<T, string> value,
        string defaultOption,
        object selectedVal)
    {
        return ToSelectList<T>(enumerable, text, value, defaultOption, selectedVal, false,null);
    }

    public static List<SelectListItem> ToSelectList<T>(
        this IEnumerable<T> enumerable,
        Func<T, string> text,
        Func<T, string> value,
        string defaultOption,
        object selectedVal,
        bool sortAlphabetically)
    {
        return ToSelectList<T>(enumerable, text, value, defaultOption, selectedVal, sortAlphabetically, null);
    }

    public static List<SelectListItem> ToSelectList<T>(
        this IEnumerable<T> enumerable,
        Func<T, string> text,
        Func<T, string> value,
        string defaultOption,
        object selectedVal,
        bool sortAlphabetically,
        object FirstValueOverride)
    {

        int iSelectedVal = -1;

        if(selectedVal!=null)
        {
            try
            {
                iSelectedVal = Convert.ToInt32(selectedVal);
            }
            catch
            {
            }
        }

        var items = enumerable.Select(f => new SelectListItem()
        {
            Text = text(f),
            Value = value(f),
            Selected = (iSelectedVal > -1)? (iSelectedVal.ToString().Equals(value(f))) : false
        });

        #region Sortare alfabetica
        if (sortAlphabetically)
            items = items.OrderBy(t => t.Text);
        #endregion Sortare alfabetica

        var itemsList = items.ToList();

        Func<SelectListItem, bool> funct = null;
        string sValue = string.Empty;
        SelectListItem firstItem = null;
        SelectListItem overridenItem = null;
        int overridenIndex = 0;

        if (FirstValueOverride != null)
        {
            sValue = FirstValueOverride.ToString();

            funct = (t => t.Value == sValue);
            overridenItem = itemsList.SingleOrDefault(funct);
            overridenIndex = itemsList.IndexOf(overridenItem);

            if (overridenItem != null)
            {
                firstItem = itemsList.ElementAt(0);
                itemsList[0] = overridenItem;
                itemsList[overridenIndex] = firstItem;
            }
        }

        if(!string.IsNullOrEmpty(defaultOption))
            itemsList.Insert(0, new SelectListItem()
            {
                Text = defaultOption,
                Value = "-1"
            });

        return itemsList;
    }
}