using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BePreferenceCentre.DAL;

namespace BePreferenceCentre.ViewModels
{
    public class AdminViewModel
    {
        public IEnumerable<BEMailingList> AdminMailingLists { get; set; }
    }
}