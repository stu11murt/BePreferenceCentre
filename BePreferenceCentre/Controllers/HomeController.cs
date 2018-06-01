using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BePreferenceCentre.DAL;
using BePreferenceCentre.ViewModels;
using System.Threading.Tasks;
using MailChimp.Net.Interfaces;
using MailChimp.Net;
using MailChimp.Net.Logic;
using BePreferenceCentre.Models;

namespace BePreferenceCentre.Controllers
{
    public class HomeController : Controller
    {
        private const string APIKEY = "beca8f48a27a32cdea3915e187823788-us16";
        IMailChimpManager manager = new MailChimpManager(APIKEY);

        public HomeController()
        {

        }

        public ActionResult Index(string email = "stu11murt@gmail.com")
        {
            List<UserSubscribedtoListViewModel> userModel = new List<UserSubscribedtoListViewModel>();
            MailingListOptionsViewModel mailListOptions = new MailingListOptionsViewModel();
            mailListOptions.CurrentlySubscribedListId = "NA";

            BePreferencesEntities db = new BePreferencesEntities();

            List<BEMemberList> listsMemberof = db.BEMemberLists.Where(u => u.Email == email).ToList();

            foreach (BEMailingList mlist in db.BEMailingLists.ToList())
            {
                UserSubscribedtoListViewModel beMaiList = new UserSubscribedtoListViewModel();

                if (listsMemberof.FirstOrDefault(m => m.ListId == mlist.ListId && m.Archived == false) != null)
                {
                    beMaiList.Subscribed = true;
                    mailListOptions.CurrentlySubscribedListId = mlist.ListId;
                }
                else
                {
                    beMaiList.Subscribed = false;
                }

                beMaiList.MailingList = mlist;
                userModel.Add(beMaiList);
            }

            mailListOptions.Email = email;
            mailListOptions.MailingLists = userModel;
            mailListOptions.MailingListMembers = db.BEMailingLists.ToList();


            return View(mailListOptions);
        }

        public async Task<ActionResult> GetMailingLists()
        {
            

            var mailChimpListCollection = await this.manager.Lists.GetAllAsync().ConfigureAwait(false);

            foreach (MailChimp.Net.Models.List lst in mailChimpListCollection)
            {
                var newMailList = new BEMailingList
                {
                    ListName = lst.Name,
                    ListId = lst.Id,
                    MemberCount = lst.Stats.MemberCount,
                    LastUpdated = DateTime.Now
                };

                BePreferencesEntities db = new BePreferencesEntities();
                db.BEMailingLists.Add(newMailList);
                db.SaveChanges();
            }

            MailingListsViewModel vwMod = new MailingListsViewModel();

            vwMod.AllLists = mailChimpListCollection;

            return View(vwMod);
        }

        public async Task<ActionResult> SubscribeUser(string listId, string email)
        {
            try
            {
                 if (listId == "NA")
                        return Content("error");

                    using (var db = new BePreferencesEntities())
                    {
                        if (db.BEMemberLists.FirstOrDefault(m => m.Email == email && m.ListId == listId && m.Archived == true) != null)
                        {
                            BEMemberList member = db.BEMemberLists.FirstOrDefault(e => e.Email == email && e.ListId == listId);
                            if (member != null)
                            {
                                // MailChimp.Net.Models.Member memberToSubscribe = await GetMember(currentlistId, email);
                                var newMember = new MailChimp.Net.Models.Member { EmailAddress = email, StatusIfNew = MailChimp.Net.Models.Status.Subscribed };

                                newMember.MergeFields.Add("FNAME", member.FirstName);
                                newMember.MergeFields.Add("LNAME", member.LastName);
                                newMember.MergeFields.Add("FULLNAME", member.FirstName + " " + member.LastName);

                                await this.manager.Members.AddOrUpdateAsync(listId, newMember);
                                ReInstateMemberToDatabase(member);


                                return RedirectToAction("Index", new { email = member.Email });
                            }
                            else
                            {
                                return RedirectToAction("Index", new { email = email });
                            }
                        }
                        else
                        {


                            BEMemberList member = db.BEMemberLists.FirstOrDefault(e => e.Email == email);
                            if (member != null)
                            {
                                // MailChimp.Net.Models.Member memberToSubscribe = await GetMember(currentlistId, email);
                                var newMember = new MailChimp.Net.Models.Member { EmailAddress = email, StatusIfNew = MailChimp.Net.Models.Status.Subscribed };

                                newMember.MergeFields.Add("FNAME", member.FirstName);
                                newMember.MergeFields.Add("LNAME", member.LastName);
                                newMember.MergeFields.Add("FULLNAME", member.FirstName + " " + member.LastName);

                                await this.manager.Members.AddOrUpdateAsync(listId, newMember);
                                AddMemberToDatabase(member, listId);


                                return RedirectToAction("Index", new { email = member.Email });
                            }
                            else
                            {
                                return RedirectToAction("Index", new { email = email });
                            }
                        }
                       
                    }
                
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        private void AddMemberToDatabase(BEMemberList member, string newListId)
        {
            try
            {
                using(var db = new BePreferencesEntities())
                {
                    BEMemberList newMember = new BEMemberList
                    {
                        Email = member.Email,
                        FirstName = member.FirstName,
                        LastName = member.LastName,
                        AccessToken = member.AccessToken,
                        Created = DateTime.Now,
                        ListId = newListId
                    };

                    db.BEMemberLists.Add(newMember);
                    db.SaveChanges();
                    
                }
            }
            catch (Exception)
            {

                throw;
            }
        }


        private void ReInstateMemberToDatabase(BEMemberList member)
        {
            try
            {
                using (var db = new BePreferencesEntities())
                {
                    member.Archived = false;

                    db.Entry(member).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<ActionResult> UnsubscribeUser(string listId, string email)
        {
            try
            {
                IMailChimpManager manager = new MailChimpManager(APIKEY);

                using (var db = new BePreferencesEntities())
                {
                    BEMemberList member = db.BEMemberLists.FirstOrDefault(e => e.Email == email);
                    if (member != null)
                    {
                        await this.manager.Members.DeleteAsync(listId, member.Email);
                                                
                    }

                    RemoveMemberToDatabase(member, listId);
                    
                    return RedirectToAction("Index", new { email = "stu11murt@gmail.com" });
                }
                
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void RemoveMemberToDatabase(BEMemberList member, string newListId)
        {
            try
            {
                using (var db = new BePreferencesEntities())
                {
                    member.ListId = newListId;
                    member.Archived = true;

                    db.Entry(member).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                }
            }
            catch (Exception)
            {

                throw;
            }
        }


        public async Task<MailChimp.Net.Models.Member> GetMember(string listId, string email)
        {
            IMailChimpManager manager = new MailChimpManager(APIKEY);

            MailChimp.Net.Models.Member SingleMember = await manager.Members.GetAsync(listId, email).ConfigureAwait(false);

            return SingleMember;
        }

        public async Task<ActionResult> GenerateMembers()
        {
            IMailChimpManager manager = new MailChimpManager(APIKEY);

            List<BEMemberList> newMemberList = new List<BEMemberList>();

            List<MailChimp.Net.Models.Member> allMembers = new List<MailChimp.Net.Models.Member>();

            var mailChimpListCollection = await manager.Lists.GetAllAsync().ConfigureAwait(false);

            foreach (MailChimp.Net.Models.List lst in mailChimpListCollection)
            {
                var lstMembers = await manager.Members.GetAllAsync(lst.Id).ConfigureAwait(false);

                allMembers.AddRange(lstMembers);
            }

            foreach (MailChimp.Net.Models.Member member in allMembers)
            {
                using (var db = new BePreferencesEntities())
                {
                    var newMember = new BEMemberList
                    {
                        FirstName = member.MergeFields.ContainsKey("FNAME") ? member.MergeFields["FNAME"].ToString() : "NA",
                        LastName = member.MergeFields.ContainsKey("LNAME") ? member.MergeFields["LNAME"].ToString() : "NA",
                        Email = member.EmailAddress,
                        AccessToken = Guid.NewGuid().ToString(),
                        Created = DateTime.Now,
                        ListId = member.ListId

                    };

                    db.BEMemberLists.Add(newMember);
                    db.SaveChanges();
                }
            }

            return View(newMemberList);
        }
    }
}