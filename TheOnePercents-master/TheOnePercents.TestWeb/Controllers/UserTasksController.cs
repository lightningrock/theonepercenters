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
using TheOnePercents.TestWeb.Models;
using WebMatrix.WebData;

namespace TheOnePercents.TestWeb.Controllers
{
    public class UserTasksController : ApiController
    {
        private DashboardContext db = new DashboardContext();

        // GET api/UserTasks
        public IQueryable<UserTasks> GetUserTasks()
        {
            var user = WebSecurity.CurrentUserId;
            return db.UserTasks.Where(t => t.UserID == user);
        }

        // GET api/UserTasks/5
        [ResponseType(typeof(UserTasks))]
        public IHttpActionResult GetUserTasks(int id)
        {
            UserTasks usertasks = db.UserTasks.Find(id);
            if (usertasks == null)
            {
                return NotFound();
            }

            return Ok(usertasks);
        }

        // PUT api/UserTasks/5
        public IHttpActionResult PutUserTasks(int id, UserTasks usertasks)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != usertasks.UserTaskID)
            {
                return BadRequest();
            }

            usertasks.UserID = WebSecurity.CurrentUserId;

            db.Entry(usertasks).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserTasksExists(id))
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

        public class FromTo
        {
            public DateTime from;
            public DateTime to;
        }

        [ResponseType(typeof(int))]
        public IHttpActionResult PostUserTasksPointsFromTo(FromTo span)
        {
            int points = 0;
            var user = WebSecurity.CurrentUserId;
            var tasks = db.UserTasks.Where(t => t.UserID == user && t.TaskDate > span.from && t.TaskDate < span.to);
            if (tasks.Count() != 0)
            {
                points = tasks.Sum(t => t.Task.Points);
            } 

            return CreatedAtRoute("DefaultApiPost", new { id = user }, points);
        }

        // POST api/UserTasks
        [ResponseType(typeof(UserTasks))]
        public IHttpActionResult PostUserTasks(UserTasks usertasks)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            usertasks.UserID = WebSecurity.CurrentUserId;
            db.UserTasks.Add(usertasks);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApiPost", new { id = usertasks.UserTaskID }, usertasks);
        }

        // DELETE api/UserTasks/5
        [ResponseType(typeof(UserTasks))]
        public IHttpActionResult DeleteUserTasks(int id)
        {
            UserTasks usertasks = db.UserTasks.Find(id);
            if (usertasks == null)
            {
                return NotFound();
            }

            db.UserTasks.Remove(usertasks);
            db.SaveChanges();

            return Ok(usertasks);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserTasksExists(int id)
        {
            return db.UserTasks.Count(e => e.UserTaskID == id) > 0;
        }
    }
}