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
using System.IO;
using OfficeOpenXml;

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

        public FileResult DownloadStores()
        {
            InkeyStoreViewModel inkViewMod = new InkeyStoreViewModel();
            byte[] fileBytes = Encoding.ASCII.GetBytes(inkViewMod.InkeyJsonStores);
            string fileName = "inkeyStores.json";
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        public FileResult DownloadStoresUS()
        {
            InkeyStoreViewModel inkViewMod = new InkeyStoreViewModel();
            byte[] fileBytes = Encoding.ASCII.GetBytes(inkViewMod.InkeyJsonStoresUS);
            string fileName = "inkeyStoresUS.json";
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
                    newAns.ProductName = db.InkeyProducts.FirstOrDefault(p => p.InkeyProductsId == inkMod.answer.ProductId).ProductName;
                    db.InkeyAnswers.Add(newAns);
                    db.SaveChanges();

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
            InkeyAnswer answerToEdit = db.InkeyAnswers.FirstOrDefault(i => i.InkeyAnswersId == id);
            //answerToEdit.ProductName = db.InkeyProducts.FirstOrDefault(p => p.InkeyProductsId == (int)answerToEdit.ProductId).ProductName;
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
                    inkAnswer.ProductName = db.InkeyProducts.FirstOrDefault(p => p.InkeyProductsId == inkAnswer.ProductId).ProductName;
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


        public ActionResult GetProductLinks(int id)
        {
            BePreferencesEntities db = new BePreferencesEntities();
            string test = db.InkeyProducts.FirstOrDefault(i => i.InkeyProductsId == id).ProductImageLink + "," + db.InkeyProducts.FirstOrDefault(i => i.InkeyProductsId == id).ProductLink + "," + db.InkeyProducts.FirstOrDefault(i => i.InkeyProductsId == id).PhoneticName;
            return Content(db.InkeyProducts.FirstOrDefault(i => i.InkeyProductsId == id).ProductImageLink + "," + db.InkeyProducts.FirstOrDefault(i => i.InkeyProductsId == id).ProductLink + "," + db.InkeyProducts.FirstOrDefault(i => i.InkeyProductsId == id).PhoneticName);
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

        #region "API test"

        public ActionResult TestAPI()
        {
            return View();
        }

        #endregion

        #region "import questions"

        public ActionResult InkeyImport()
        {
            return View();
        }


        [HttpPost]
        public ActionResult ReadExcel(System.Web.HttpPostedFileBase upload)
        {

            try
            {
                string test = Path.GetExtension(upload.FileName);
                if (Path.GetExtension(upload.FileName) == ".xlsx" || Path.GetExtension(upload.FileName) == ".xls")
                {
                    ExcelPackage package = new ExcelPackage(upload.InputStream);

                    List<InkeyAnswer> questions = new List<InkeyAnswer>();

                    questions = ExcelHelper.PopulateAnswers(package.Workbook.Worksheets.FirstOrDefault(), true).ToList();

                    CheckQuestionsandAddtoDatabase(questions);
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }

            return View("InkeyImport");
        }

        public ActionResult ImportStores()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ReadExcelStores(System.Web.HttpPostedFileBase upload)
        {

            try
            {
                string test = Path.GetExtension(upload.FileName);
                if (Path.GetExtension(upload.FileName) == ".xlsx" || Path.GetExtension(upload.FileName) == ".xls")
                {
                    ExcelPackage package = new ExcelPackage(upload.InputStream);

                    List<InkeyStore> stores = new List<InkeyStore>();

                    stores = ExcelHelper.PopulateStoresAmended(package.Workbook.Worksheets.FirstOrDefault(), true).ToList();

                    CheckStoresAndAddtoDatabase(stores);
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }

            return View("InkeyImport");
        }

        [HttpPost]
        public ActionResult ReadExcelStoresUS(System.Web.HttpPostedFileBase upload)
        {

            try
            {
                string test = Path.GetExtension(upload.FileName);
                if (Path.GetExtension(upload.FileName) == ".xlsx" || Path.GetExtension(upload.FileName) == ".xls")
                {
                    ExcelPackage package = new ExcelPackage(upload.InputStream);

                    List<InkeyStoresU> stores = new List<InkeyStoresU>();

                    stores = ExcelHelper.PopulateStoresUS(package.Workbook.Worksheets.FirstOrDefault(), true).ToList();

                    CheckStoresAndAddtoDatabaseUS(stores);
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }

            return View("InkeyImport");
        }



        private void CheckQuestionsandAddtoDatabase(List<InkeyAnswer> attendeesToAdd)
        {
            try
            {
                using (var db = new BePreferencesEntities())
                {
                    foreach (InkeyAnswer question in attendeesToAdd)
                    {
                        db.InkeyAnswers.Add(question);
                        db.SaveChanges();

                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void CheckStoresAndAddtoDatabase(List<InkeyStore> storesToAdd)
        {
            try
            {
                using (var db = new BePreferencesEntities())
                {
                    foreach (InkeyStore store in storesToAdd)
                    {
                        db.InkeyStores.Add(store);
                        db.SaveChanges();

                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void CheckStoresAndAddtoDatabaseUS(List<InkeyStoresU> storesToAdd)
        {
            try
            {
                using (var db = new BePreferencesEntities())
                {
                    foreach (InkeyStoresU store in storesToAdd)
                    {
                        db.InkeyStoresUS.Add(store);
                        db.SaveChanges();

                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #endregion

        #region "Inkey User Questions"

        public ActionResult UserQuestions()
        {
            BePreferencesEntities db = new BePreferencesEntities();
            return View(db.InkeyUserQuestions.ToList());
        }

        #endregion

    }
}