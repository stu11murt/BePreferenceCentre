using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Cryptography;
using BePreferenceCentre.Helpers;
using BePreferenceCentre.DAL;
using BePreferenceCentre.ViewModels;
using BePreferenceCentre.ActionFilters;
using System.Text;

namespace BePreferenceCentre.Controllers
{
    public class AdminController : Controller
    {
        internal const string Inputkey = "560A18CD-6346-4CF0-A2E8-671F9B6B9EA9";

        // GET: Admin
        public ActionResult Index()
        {
           
            List<UserSubscribedtoListViewModel> userModel = new List<UserSubscribedtoListViewModel>();
            MailingListOptionsViewModel mailListOptions = new MailingListOptionsViewModel();
            mailListOptions.CurrentlySubscribedListId = "NA";

            BePreferencesEntities db = new BePreferencesEntities();

            List<BEMailingList> list = db.BEMailingLists.ToList();

            foreach(BEMailingList mlist in list)
            {
                UserSubscribedtoListViewModel beMaiList = new UserSubscribedtoListViewModel();

                beMaiList.MailingList = mlist;

                userModel.Add(beMaiList);
            }

           
            mailListOptions.MailingLists = userModel;
            mailListOptions.MailingListMembers = db.BEMailingLists.ToList();


            return View(mailListOptions);
           
        }


        public ActionResult SendSimpleEmail(string email)
        {

           string encryptEmail = EncryptionHelper.EncryptRijndael(email);

            SimpleEmailSender.SendSimpleEmail(email, HttpUtility.UrlEncode(encryptEmail));
            
            return View();
        }


        [AllowCORSFilter]
        public ActionResult BeBot()
        {
            return View();
        }
        
        public ActionResult InkeyHelp()
        {
            InkeyViewModel inkViewMod = new InkeyViewModel();
            return View(inkViewMod);
        }

        public ActionResult InkeySearch()
        {
            InkeyViewModel inkViewMod = new InkeyViewModel();
            return View(inkViewMod);
        }


        public ActionResult InkeyAnswerForm()
        {
            InkeyViewModel inkViewMod = new InkeyViewModel();
            return View(inkViewMod);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult InkeyAnswerForm(InkeyViewModel inkMod)
        {
            try
            {
                InkeyAnswer newAns = inkMod.answer;
                using(var db = new BePreferencesEntities())
                {
                    //db.InkeyAnswers.Add(newAns);
                    //db.SaveChanges();

                    return View(new InkeyViewModel());
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public FileResult Download()
        {
            InkeyViewModel inkViewMod = new InkeyViewModel();
            byte[] fileBytes = Encoding.ASCII.GetBytes(inkViewMod.InkeyJsonAnswers);
            string fileName = "inkey.json";
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }


        public ActionResult Animated()
        {
            return View();
        }

    }
}