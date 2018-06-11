using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Cryptography;
using BePreferenceCentre.Helpers;

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


        public ActionResult SendSimpleEmail()
        {
            SimpleEmailSender.SendSimpleEmail("stu11murt@gmail.com");

            var encryption = EncryptionHelper.EncryptRijndael("stu11murt@gmail.com", Guid.NewGuid().ToString());
            return View();
        }
    }
}