using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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

    public class newGrades
    {
        public int Id;
        public String courseID;
        public String studentID;
        public int? aGrade;
        public int? bGrade;
    }

    public class GradesController : Controller
    {
        private GradeDal db = new GradeDal();
        private CourseDal cdb = new CourseDal();
        private UserDal sdb = new UserDal();
        private scLinkDal scdb = new scLinkDal();


        // GET: Grades
        public ActionResult Index(int cid = 0)
        {
            ViewBag.session = Session["user"];
            if (ViewBag.session == null || ViewBag.session.Rank < 2)
            {
                return View("Unauth");
            }
            List<newGrades> res = new List<newGrades>();

            foreach (Grades g in db.Grades.Where(g => g.courseID == cid).ToList())
            {
                newGrades temp = new newGrades();
                temp.Id = g.Id;
                temp.courseID = cdb.Course.Find(g.courseID).name;
                temp.studentID = sdb.User.FirstOrDefault(u => u.StudentID == g.studentID).FullName;
                temp.aGrade = g.aGrade;
                temp.bGrade = g.bGrade;
                res.Add(temp);
            }
            ViewBag.cid = cid;
            return View(res);
        }


        // GET: Grades/Edit/5
        public ActionResult Edit(int? id, int cid = 0)
        {
            ViewBag.session = Session["user"];
            if (ViewBag.session == null || ViewBag.session.Rank < 2)
            {
                return View("Unauth");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Grades grades = db.Grades.Find(id);
            if (grades == null)
            {
                return HttpNotFound();
            }

            ViewBag.courses = cdb.Course.Where(c => c.Id == cid).ToList();
            List<User> students = new List<User>();
            List<scLink> scLinkList = scdb.scLink.ToList();
            foreach (var a in sdb.User.ToList())
            {
                foreach(var b in scLinkList.Where(bd => bd.courseID == cid).ToList())
                {
                    if(b.studentID == a.StudentID)
                    {
                        students.Add(a);
                    }
                }
            }
            ViewBag.students = students;
            ViewBag.selected = grades;
            ViewBag.link = "/Grades/?cid=" + cid.ToString();
            return View(grades);
        }

        // POST: Grades/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,courseID,studentID,aGrade,bGrade")] Grades grades)
        {
            if (ModelState.IsValid)
            {
                db.Entry(grades).State = EntityState.Modified;
                db.SaveChanges();
                return Redirect("/Grades?cid="+Request["courseID"]);
            }
            return Redirect("/Grades/Edit?id=" + Request["Id"] + "&cid=" + Request["courseID"]);
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
