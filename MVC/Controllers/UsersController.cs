using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVC.Dal;
using MVC.Models;

namespace MVC.Controllers
{
    public class UsersController : Controller
    {
        private UserDal db = new UserDal();

        // GET: Users
        public ActionResult Index()
        {
            ViewBag.session = Session["user"];
            if (ViewBag.session == null || ViewBag.session.Rank < 3)
            {
                return View("Unauth");
            }
            return View(db.User.ToList());
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            ViewBag.session = Session["user"];
            if (ViewBag.session == null || ViewBag.session.Rank < 3)
            {
                return View("Unauth");
            }
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,StudentID,Password,Email,FullName,Rank")] User user)
        {
            List<User> tmp = db.User.Where(x => x.StudentID == user.StudentID).ToList();
            if(tmp.Count != 0)
            {
                ViewBag.error = "User with that id already exists";
                return View(user);
            }
            if (ModelState.IsValid)
            {
                db.User.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.session = Session["user"];
            if (ViewBag.session == null || (ViewBag.session.Id != id && ViewBag.session.Rank < 3))
            {
                return View("Unauth");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.User.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.user = user;
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,StudentID,Password,Email,FullName,Rank")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                ViewBag.session = Session["user"];
                if (ViewBag.session == null || ViewBag.session.Rank < 3)
                {
                    return Redirect("/Home");
                }
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            ViewBag.session = Session["user"];
            if (ViewBag.session == null || ViewBag.session.Rank < 3)
            {
                return View("Unauth");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.User.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.User.Find(id);
            db.User.Remove(user);
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
