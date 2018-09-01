using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using BePreferenceCentre.DAL;
using System.Net.Mail;

namespace BePreferenceCentre.Controllers
{
    public class InkeyUserQuestionsEmailController : ApiController
    {
        private BePreferencesEntities db = new BePreferencesEntities();

        // GET: api/InkeyUserQuestionsEmail
        public IQueryable<InkeyUserQuestion> GetInkeyUserQuestions()
        {
            return db.InkeyUserQuestions;
        }

        // GET: api/InkeyUserQuestionsEmail/5
        [ResponseType(typeof(InkeyUserQuestion))]
        public async Task<IHttpActionResult> GetInkeyUserQuestion(int id)
        {
            InkeyUserQuestion inkeyUserQuestion = await db.InkeyUserQuestions.FindAsync(id);
            if (inkeyUserQuestion == null)
            {
                return NotFound();
            }

            return Ok(inkeyUserQuestion);
        }

        // PUT: api/InkeyUserQuestionsEmail/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutInkeyUserQuestion(int id, InkeyUserQuestion inkeyUserQuestion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != inkeyUserQuestion.InkeyUserQuestionsId)
            {
                return BadRequest();
            }

            db.Entry(inkeyUserQuestion).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InkeyUserQuestionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/InkeyUserQuestionsEmail
        [ResponseType(typeof(InkeyUserQuestion))]
        public async Task<IHttpActionResult> PostInkeyUserQuestion(InkeyUserQuestion inkeyUserQuestion)
        {
            if (!IsValid(inkeyUserQuestion.userEmail))
                return BadRequest("invalid email");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            InkeyUserQuestion userQuestion = db.InkeyUserQuestions.FirstOrDefault(q => q.InkeyUserQuestionsId == inkeyUserQuestion.InkeyUserQuestionsId);
            if (userQuestion != null)
            {
                userQuestion.userEmail = inkeyUserQuestion.userEmail;

                db.Entry(userQuestion).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return CreatedAtRoute("DefaultApi", new { id = inkeyUserQuestion.InkeyUserQuestionsId }, inkeyUserQuestion);
            }

            return BadRequest(ModelState);
        }

        // DELETE: api/InkeyUserQuestionsEmail/5
        [ResponseType(typeof(InkeyUserQuestion))]
        public async Task<IHttpActionResult> DeleteInkeyUserQuestion(int id)
        {
            InkeyUserQuestion inkeyUserQuestion = await db.InkeyUserQuestions.FindAsync(id);
            if (inkeyUserQuestion == null)
            {
                return NotFound();
            }

            db.InkeyUserQuestions.Remove(inkeyUserQuestion);
            await db.SaveChangesAsync();

            return Ok(inkeyUserQuestion);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool InkeyUserQuestionExists(int id)
        {
            return db.InkeyUserQuestions.Count(e => e.InkeyUserQuestionsId == id) > 0;
        }

        public bool IsValid(string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}