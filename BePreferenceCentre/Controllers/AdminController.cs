﻿using System;
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

        public ActionResult SummerNote()
        {
            InkeyAnswer inkAns = new InkeyAnswer();
            return View(inkAns);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SummerNote(InkeyAnswer inkAns)
        {
            return View(inkAns);
        }

        #region "Inkey Questions"

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
                using (var db = new BePreferencesEntities())
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

        public ActionResult EditQuestion(int id)
        {
            BePreferencesEntities db = new BePreferencesEntities();
            return View(db.InkeyAnswers.FirstOrDefault(i => i.InkeyAnswersId == id));
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult EditQuestion(InkeyAnswer inkAnswer)
        {
            try
            {
                using (var db = new BePreferencesEntities())
                {
                    db.Entry(inkAnswer).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }

                return RedirectToAction("InkeyAnswerForm");
            }
            catch (Exception)
            {

                throw;
            }
        }


        public ActionResult GetProductLink(int id)
        {
            BePreferencesEntities db = new BePreferencesEntities();
            return Content(db.InkeyProducts.FirstOrDefault(i => i.InkeyProductsId == id).ProductImageLink);
        }

        #endregion

        #region "Inkey Products"

        public ActionResult ProductManager()
        {
            ProductManagerViewModel prodManViewMod = new ProductManagerViewModel();
            return View(prodManViewMod);
        }

        [HttpPost]
        public ActionResult AddNewProduct(ProductManagerViewModel newProdMod)
        {
            try
            {
                using(var db = new BePreferencesEntities())
                {
                    InkeyProduct newProd = newProdMod.NewProduct;

                    db.InkeyProducts.Add(newProd);
                    db.SaveChanges();

                    return RedirectToAction("ProductManager");
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public ActionResult RemoveProduct(int prodId)
        {
            try
            {
                using (var db = new BePreferencesEntities())
                {
                    InkeyProduct newProd = db.InkeyProducts.FirstOrDefault(p => p.InkeyProductsId == prodId);

                    db.InkeyProducts.Remove(newProd);
                    db.SaveChanges();

                    return RedirectToAction("ProductManager");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }


        public ActionResult EditProduct(int id)
        {
            BePreferencesEntities db = new BePreferencesEntities();
            return PartialView("Partials/_EditProduct", db.InkeyProducts.FirstOrDefault(s => s.InkeyProductsId == id));
        }

        [HttpPost]
        public ActionResult EditProduct(InkeyProduct inkProduct)
        {
            try
            {
                using (var db = new BePreferencesEntities())
                {
                    db.Entry(inkProduct).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                }

                return RedirectToAction("ProductManager");
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

    }
}