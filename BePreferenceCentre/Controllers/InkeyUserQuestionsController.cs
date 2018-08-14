using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using BePreferenceCentre.DAL;
using Newtonsoft.Json;

namespace BePreferenceCentre.Controllers
{
    public class InkeyUserQuestionsController : ApiController
    {
        private BePreferencesEntities db = new BePreferencesEntities();

        // GET: api/InkeyUserQuestions
        //public IQueryable<InkeyUserQuestion> GetInkeyUserQuestions()
        //{
        //    return db.InkeyUserQuestions;
        //}

        [ResponseType(typeof(string))]
        public string GetInkeyUserQuestionsJSON()
        {
            return JsonConvert.SerializeObject(db.InkeyAnswers, Formatting.None);
        }

        // GET: api/InkeyUserQuestions/5
        [ResponseType(typeof(InkeyUserQuestion))]
        public IHttpActionResult GetInkeyUserQuestion(int id)
        {
            InkeyUserQuestion inkeyUserQuestion = db.InkeyUserQuestions.Find(id);
            if (inkeyUserQuestion == null)
            {
                return NotFound();
            }

            return Ok(inkeyUserQuestion);
        }

        // PUT: api/InkeyUserQuestions/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutInkeyUserQuestion(int id, InkeyUserQuestion inkeyUserQuestion)
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
                db.SaveChanges();
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

        // POST: api/InkeyUserQuestions
        [ResponseType(typeof(InkeyUserQuestion))]
        public IHttpActionResult PostInkeyUserQuestion(InkeyUserQuestion inkeyUserQuestion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            inkeyUserQuestion.Created = DateTime.Now;
            db.InkeyUserQuestions.Add(inkeyUserQuestion);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = inkeyUserQuestion.InkeyUserQuestionsId }, inkeyUserQuestion);
        }

        // DELETE: api/InkeyUserQuestions/5
        [ResponseType(typeof(InkeyUserQuestion))]
        public IHttpActionResult DeleteInkeyUserQuestion(int id)
        {
            InkeyUserQuestion inkeyUserQuestion = db.InkeyUserQuestions.Find(id);
            if (inkeyUserQuestion == null)
            {
                return NotFound();
            }

            db.InkeyUserQuestions.Remove(inkeyUserQuestion);
            db.SaveChanges();

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
    }
}