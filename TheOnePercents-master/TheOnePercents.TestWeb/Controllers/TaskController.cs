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

namespace TheOnePercents.TestWeb.Controllers
{
    public class TaskController : ApiController
    {
        private DashboardContext db = new DashboardContext();

        // GET api/Task
        public IQueryable<Task> GetTask()
        {
            var ret = db.Task;
            return ret;
        }

        // GET api/Task
        public IQueryable<Task> GetActiveTask()
        {
            var ret = db.Task.Where(t => t.Active == true);
            return ret;
        }

        // GET api/Task/5
        [ResponseType(typeof(Task))]
        public IHttpActionResult GetTask(int id)
        {
            Task task = db.Task.Find(id);
            if (task == null)
            {
                return NotFound();
            }

            return Ok(task);
        }

        // PUT api/Task/5
        public IHttpActionResult PutTask(int id, Task task)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != task.TaskID)
            {
                return BadRequest();
            }

            db.Entry(task).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskExists(id))
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

        // POST api/Task
        [ResponseType(typeof(Task))]
        public IHttpActionResult PostTask(Task task)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Task.Add(task);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = task.TaskID }, task);
        }

        // DELETE api/Task/5
        [ResponseType(typeof(Task))]
        public IHttpActionResult DeleteTask(int id)
        {
            Task task = db.Task.Find(id);
            if (task == null)
            {
                return NotFound();
            }

            db.Task.Remove(task);
            db.SaveChanges();

            return Ok(task);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TaskExists(int id)
        {
            return db.Task.Count(e => e.TaskID == id) > 0;
        }
    }
}