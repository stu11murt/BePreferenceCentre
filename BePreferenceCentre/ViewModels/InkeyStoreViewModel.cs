using System.Collections.Generic;
using System.Linq;
using BePreferenceCentre.DAL;
using Newtonsoft.Json;

namespace BePreferenceCentre.ViewModels
{
    public class InkeyStoreViewModel
    {
        BePreferencesEntities db = new BePreferencesEntities();
        public InkeyStoreViewModel()
        {
            Store = new InkeyStore();
            InkeyStores = db.InkeyStores.ToList();
            InkeyJsonStores = JsonConvert.SerializeObject(db.InkeyStores.ToArray());

            //us stores
            StoreUS = new InkeyStoresU();
            InkeyStoresUS = db.InkeyStoresUS.ToList();
            InkeyJsonStoresUS = JsonConvert.SerializeObject(db.InkeyStoresUS.ToArray());
        }


        public List<InkeyStore> InkeyStores { get; set; }
        public List<InkeyStoresU> InkeyStoresUS { get; set; }

        public InkeyStore Store { get; set; }
        public InkeyStoresU StoreUS { get; set; }

        public string InkeyJsonStores { get; set; }
        public string InkeyJsonStoresUS { get; set; }
    }
}