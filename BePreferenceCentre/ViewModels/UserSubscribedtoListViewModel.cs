using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BePreferenceCentre.DAL;

namespace BePreferenceCentre.ViewModels
{
    public class UserSubscribedtoListViewModel
    {
        public BEMailingList MailingList { get; set; }
        public bool Subscribed { get; set; }
    }
}