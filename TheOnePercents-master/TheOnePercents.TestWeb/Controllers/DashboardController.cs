using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TheOnePercents.TestWeb.Models;

namespace TheOnePercents.TestWeb.Controllers
{
    public class DashboardController : Controller
    {
        private DashboardContext db = new DashboardContext();

        // GET: /Default1/
        [Authorize]
        public ActionResult Index()
        {
            return View(db.UserTasks.ToList());
            //return View();
        }

        // GET: /Default1/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserTasks usertasks = db.UserTasks.Find(id);
            if (usertasks == null)
            {
                return HttpNotFound();
            }
            return View(usertasks);
        }

        // GET: /Default1/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Default1/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include="TaskID,date,UserID,Completed")] UserTasks usertasks)
        {
            if (ModelState.IsValid)
            {
                usertasks.TaskDate = DateTime.UtcNow;
                db.UserTasks.Add(usertasks);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(usertasks);
        }

        // GET: /Default1/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserTasks usertasks = db.UserTasks.Find(id);
            if (usertasks == null)
            {
                return HttpNotFound();
            }
            return View(usertasks);
        }

        // POST: /Default1/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include="TaskID,date,UserID,Completed")] UserTasks usertasks)
        {
            if (ModelState.IsValid)
            {
                db.Entry(usertasks).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(usertasks);
        }

        // GET: /Default1/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserTasks usertasks = db.UserTasks.Find(id);
            if (usertasks == null)
            {
                return HttpNotFound();
            }
            return View(usertasks);
        }

        // POST: /Default1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            UserTasks usertasks = db.UserTasks.Find(id);
            db.UserTasks.Remove(usertasks);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
