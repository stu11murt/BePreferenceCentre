using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MailChimp;

namespace BePreferenceCentre.ViewModels
{
    public class MailingListsViewModel
    {
        public IEnumerable<MailChimp.Net.Models.List> AllLists { get; set; }

        public MailChimp.Net.Models.List SingleList { get; set; }

        public IEnumerable<MailChimp.Net.Models.Member> AllMembersofList { get; set; }

        public MailChimp.Net.Models.Member SingleMember { get; set; }

        public string ListName { get; set; }

    }
}