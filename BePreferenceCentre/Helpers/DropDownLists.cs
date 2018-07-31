using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BePreferenceCentre.DAL;

namespace BePreferenceCentre.Helpers
{
    public class DropDownLists
    {
        public static List<SelectListItem> GetDropDownListForInkeyProducts()
        {
            BePreferencesEntities db = new BePreferencesEntities();
            return db.InkeyProducts.Select(r => new SelectListItem { Text = r.ProductName, Value = r.InkeyProductsId.ToString() }).ToList();
        }
    }
}