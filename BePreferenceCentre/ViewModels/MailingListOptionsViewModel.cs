using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BePreferenceCentre.DAL;

namespace BePreferenceCentre.ViewModels
{
    public class MailingListOptionsViewModel
    {
       public string Email { get; set; }
       public string CurrentlySubscribedListId { get; set; }

       public List<BEMailingList> MailingListMembers { get; set; }

       public List<UserSubscribedtoListViewModel> MailingLists { get; set; }
    }
}