using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BePreferenceCentre.DAL;


namespace BePreferenceCentre.ViewModels
{
    public class ProductManagerViewModel
    {
        BePreferencesEntities db = new BePreferencesEntities();

        public ProductManagerViewModel()
        {
            AllProducts = db.InkeyProducts.ToList();
            //NewProduct = new InkeyProduct();
        }

        public InkeyProduct NewProduct { get; set; }
        public List<InkeyProduct> AllProducts { get; set; }
    }
}