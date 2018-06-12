using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Cryptography;
using BePreferenceCentre.Helpers;
using BePreferenceCentre.DAL;

namespace BePreferenceCentre.Controllers
{
    public class AdminController : Controller
    {
        internal const string Inputkey = "560A18CD-6346-4CF0-A2E8-671F9B6B9EA9";

        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult SendSimpleEmail(string email)
        {

           string encryptEmail = EncryptionHelper.EncryptRijndael(email);

            SimpleEmailSender.SendSimpleEmail(email, HttpUtility.UrlEncode(encryptEmail));
            
            return View();
        }


               

    }
}