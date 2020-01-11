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
    public class newCourse
    {
        public int Id { get; set; }
        public int startTime { get; set; }
        public int duration { get; set; }
        public String teacher { get; set; }
        public String name { get; set; }
        public String classroom { get; set; }
        public String classrooma { get; set; }
        public String classroomb { get; set; }
        public int day { get; set; }
        public DateTime aDate { get; set; }
        public DateTime bDate { get; set; }
    }

    public class CoursesController : Controller
    {
        private CourseDal db = new CourseDal();
        private GradeDal gdb = new GradeDal();
        private UserDal sdb = new UserDal();
        private scLinkDal scdb = new scLinkDal();

        protected bool Checkuser(int studentID, Course course)
        {
            List<scLink> list = scdb.scLink.Where(s => s.studentID == studentID).ToList();
            List<Course> list2 = new List<Course>();
            foreach (scLink a in list)
            {
                foreach (Course c in db.Course.AsNoTracking().ToList())
                {
                    if(c.Id == a.courseID)
                    {
                        list2.Add(c);
                    }
                }
            }
            foreach (Course c in list2)
            {

                if (c.Id != course.Id)
                {
                    if (course.startTime > c.startTime && course.startTime < c.startTime + c.duration)
                    {
                        ViewBag.error = "This theacer have a course starting at " + (c.startTime / 60).ToString() + ":" + (c.startTime % 60).ToString() + " and ends in " + ((c.startTime + c.duration) / 60).ToString() + ":" + ((c.startTime + c.duration) % 60).ToString();
                        return false;
                    }

                    if (course.startTime + course.duration > c.startTime && course.startTime + course.duration < c.startTime + c.duration)
                    {
                        ViewBag.error = "This theacer have a course starting at " + (c.startTime / 60).ToString() + ":" + (c.startTime % 60).ToString() + " and ends in " + ((c.startTime + c.duration) / 60).ToString() + ":" + ((c.startTime + c.duration) % 60).ToString();
                        return false;
                    }

                    if (c.startTime > course.startTime && c.startTime - course.startTime < course.duration)
                    {
                        ViewBag.error = "This theacer have a course starting at " + (c.startTime / 60).ToString() + ":" + (c.startTime % 60).ToString() + " and ends in " + ((c.startTime + c.duration) / 60).ToString() + ":" + ((c.startTime + c.duration) % 60).ToString();
                        return false;
                    }
                }
            }

            return true;
        }

        // GET: Courses
        public ActionResult Index()
        {
            ViewBag.session = Session["user"];
            if (ViewBag.session == null || ViewBag.session.Rank < 2)
            {
                return View("Unauth");
            }
            List<newCourse> res = new List<newCourse>();
            List<newCourse> res2 = new List<newCourse>();
            
            foreach (Course c in db.Course.ToList())
            {
                newCourse temp = new newCourse();
                temp.Id = c.Id;
                temp.startTime = c.startTime;
                temp.duration = c.duration;
                temp.teacher = sdb.User.FirstOrDefault(u => u.StudentID == c.teacher).FullName;
                temp.day = c.day;
                temp.name = c.name;
                temp.classroom = c.classroom;
                temp.classrooma = c.classrooma;
                temp.classroomb = c.classroomb;
                temp.aDate = c.aDate;
                temp.bDate = c.bDate;
                res.Add(temp);
                if (ViewBag.session.StudentID == c.teacher)
                {
                    res2.Add(temp);
                }
            }
            
            return View(ViewBag.session.Rank > 2 ? res : res2);
        }

        // GET: Courses/Create
        public ActionResult Create()
        {
            ViewBag.session = Session["user"];
            if (ViewBag.session == null || ViewBag.session.Rank < 3)
            {
                return View("Unauth");
            }
            ViewBag.teachers = sdb.User.Where(u => u.Rank == 2).ToList();
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,startTime,duration,teacher,classroom,classrooma,classroomb,name,day,aDate,bDate")] Course course)
        {
            course.startTime = Int32.Parse(Request["hour"]) * 60 + Int32.Parse(Request["min"]);
            ViewBag.edit = course;
            ViewBag.teachers = sdb.User.Where(u => u.Rank == 2).ToList();
            foreach (Course c in db.Course.AsNoTracking().Where(c => c.day == course.day).ToList())
            {

                if (c.Id != course.Id)
                {
                    if (c.teacher == course.teacher)
                    {
                        if (course.startTime > c.startTime && course.startTime < c.startTime + c.duration)
                        {
                            ViewBag.error = "This theacer have a course starting at " + (c.startTime / 60).ToString() + ":" + (c.startTime % 60).ToString() + " and ends in " + ((c.startTime + c.duration) / 60).ToString() + ":" + ((c.startTime + c.duration) % 60).ToString();
                            return View(course);
                        }

                        if (course.startTime + course.duration > c.startTime && course.startTime + course.duration < c.startTime + c.duration)
                        {
                            ViewBag.error = "This theacer have a course starting at " + (c.startTime / 60).ToString() + ":" + (c.startTime % 60).ToString() + " and ends in " + ((c.startTime + c.duration) / 60).ToString() + ":" + ((c.startTime + c.duration) % 60).ToString();
                            return View(course);
                        }

                        if (c.startTime > course.startTime && c.startTime - course.startTime < course.duration)
                        {
                            ViewBag.error = "This theacer have a course starting at " + (c.startTime / 60).ToString() + ":" + (c.startTime % 60).ToString() + " and ends in " + ((c.startTime + c.duration) / 60).ToString() + ":" + ((c.startTime + c.duration) % 60).ToString();
                            return View(course);
                        }
                    }

                    if (c.classroom == course.classroom)
                    {
                        if (course.startTime > c.startTime && course.startTime < c.startTime + c.duration)
                        {
                            ViewBag.error = "This classroom have a course starting at " + (c.startTime / 60).ToString() + ":" + (c.startTime % 60).ToString() + " and ends in " + ((c.startTime + c.duration) / 60).ToString() + ":" + ((c.startTime + c.duration) % 60).ToString();
                            return View(course);
                        }

                        if (course.startTime + course.duration > c.startTime && course.startTime + course.duration < c.startTime + c.duration)
                        {
                            ViewBag.error = "This classroom have a course starting at " + (c.startTime / 60).ToString() + ":" + (c.startTime % 60).ToString() + " and ends in " + ((c.startTime + c.duration) / 60).ToString() + ":" + ((c.startTime + c.duration) % 60).ToString();
                            return View(course);
                        }

                        if (c.startTime > course.startTime && c.startTime - course.startTime < course.duration)
                        {
                            ViewBag.error = "This classroom have a course starting at " + (c.startTime / 60).ToString() + ":" + (c.startTime % 60).ToString() + " and ends in " + ((c.startTime + c.duration) / 60).ToString() + ":" + ((c.startTime + c.duration) % 60).ToString();
                            return View(course);
                        }
                    }
                }
            }

            if (ModelState.IsValid)
            {
                db.Course.Add(course);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            
            return View(course);
        }

        // GET: Courses/Edit/5
        public ActionResult Edit(int? id)
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
            Course course = db.Course.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            ViewBag.edit = course;
            ViewBag.teachers = sdb.User.Where(u => u.Rank == 2).ToList();
            return View(course);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,startTime,duration,teacher,name,classroom,classrooma,classroomb,day,aDate,bDate")] Course course)
        {
            course.startTime = Int32.Parse(Request["hour"]) * 60 + Int32.Parse(Request["min"]);
            ViewBag.edit = course;
            ViewBag.teachers = sdb.User.Where(u => u.Rank == 2).ToList();
            foreach (Course c in db.Course.AsNoTracking().Where(c => c.day == course.day).ToList()) {
            
                if(c.Id != course.Id)
                {
                    if(c.teacher == course.teacher)
                    {
                        if (course.startTime > c.startTime && course.startTime < c.startTime + c.duration)
                        {
                            ViewBag.error = "This theacer have a course starting at " + (c.startTime / 60).ToString() + ":" + (c.startTime % 60).ToString() + " and ends in " + ((c.startTime + c.duration) / 60).ToString() + ":" + ((c.startTime + c.duration) % 60).ToString();
                            return View(course);
                        }

                        if (course.startTime + course.duration > c.startTime && course.startTime + course.duration < c.startTime + c.duration)
                        {
                            ViewBag.error = "This theacer have a course starting at " + (c.startTime / 60).ToString() + ":" + (c.startTime % 60).ToString() + " and ends in " + ((c.startTime + c.duration) / 60).ToString() + ":" + ((c.startTime + c.duration) % 60).ToString();
                            return View(course);
                        }

                        if (c.startTime > course.startTime && c.startTime - course.startTime < course.duration)
                        {
                            ViewBag.error = "This theacer have a course starting at " + (c.startTime / 60).ToString() + ":" + (c.startTime % 60).ToString() + " and ends in " + ((c.startTime + c.duration) / 60).ToString() + ":" + ((c.startTime + c.duration) % 60).ToString();
                            return View(course);
                        }
                    }

                    if(c.classroom == course.classroom)
                    {
                        if (course.startTime > c.startTime && course.startTime < c.startTime + c.duration)
                        {
                            ViewBag.error = "This classroom have a course starting at " + (c.startTime / 60).ToString() + ":" + (c.startTime % 60).ToString() + " and ends in " + ((c.startTime + c.duration) / 60).ToString() + ":" + ((c.startTime + c.duration) % 60).ToString();
                            return View(course);
                        }

                        if (course.startTime + course.duration > c.startTime && course.startTime + course.duration < c.startTime + c.duration)
                        {
                            ViewBag.error = "This classroom have a course starting at " + (c.startTime / 60).ToString() + ":" + (c.startTime % 60).ToString() + " and ends in " + ((c.startTime + c.duration) / 60).ToString() + ":" + ((c.startTime + c.duration) % 60).ToString();
                            return View(course);
                        }

                        if (c.startTime > course.startTime && c.startTime - course.startTime < course.duration)
                        {
                            ViewBag.error = "This classroom have a course starting at " + (c.startTime / 60).ToString() + ":" + (c.startTime % 60).ToString() + " and ends in " + ((c.startTime + c.duration) / 60).ToString() + ":" + ((c.startTime + c.duration) % 60).ToString();
                            return View(course);
                        }
                    }
                }
            }

            foreach (scLink user in scdb.scLink.Where(u => u.courseID == course.Id).ToList())
            {
                if (!Checkuser(user.studentID, course))
                {
                    ViewBag.error = "the student " + user.studentID.ToString() + " got class in this hour";
                    return View(course);
                }
            }
            

            if (ModelState.IsValid)
            {
                db.Entry(course).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(course);
        }

        // GET: Courses/Delete/5
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
            Course course = db.Course.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Course course = db.Course.Find(id);
            db.Course.Remove(course);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult AddToCourse(int? cid)
        {
            ViewBag.session = Session["user"];
            if (ViewBag.session == null || ViewBag.session.Rank < 3)
            {
                return View("Unauth");
            }
            if (cid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            scLink course = scdb.scLink.FirstOrDefault(u => u.courseID == cid);
            if (course == null)
            {
                course = new scLink();
                course.courseID = (int)cid;
            }
            ViewBag.courses = db.Course.Where(c => c.Id == cid).ToList();
            List<User> students = new List<User>();
            List<scLink> scLinkList = scdb.scLink.ToList();
            foreach (var a in sdb.User.ToList())
            {
                if (!scLinkList.Any(x => x.studentID == a.StudentID && x.courseID == cid) && a.Rank < 2)
                {
                    students.Add(a);
                }
            }
            if(students.Count == 0)
            {
                students = new List<User>();
            }
            ViewBag.students = students;
            return View(course);
        }

        public ActionResult RemoveFromCourse(int? cid)
        {
            ViewBag.session = Session["user"];
            if (ViewBag.session == null || ViewBag.session.Rank < 3)
            {
                return View("Unauth");
            }
            if (cid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            scLink course = scdb.scLink.FirstOrDefault(u => u.courseID == cid);
           
            ViewBag.courses = db.Course.Where(c => c.Id == cid).ToList();
            List<User> students = new List<User>();
            List<scLink> scLinkList = scdb.scLink.ToList();
            foreach (var a in sdb.User.ToList())
            {
                if (scLinkList.Any(x => x.studentID == a.StudentID && x.courseID == cid) && a.Rank < 2)
                {
                    students.Add(a);
                }
            }
            if (students.Count == 0)
            {
                students = new List<User>();
            }
            ViewBag.students = students;
            return View(course);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddToCourse([Bind(Include = "studentID, courseID")] scLink course)
        {
            ViewBag.session = Session["user"];
            if (ViewBag.session == null || ViewBag.session.Rank < 3)
            {
                return View("Unauth");
            }

            Grades grade = new Grades();
            grade.courseID = course.courseID;
            grade.studentID = course.studentID;


            if (!Checkuser(course.studentID, db.Course.FirstOrDefault(c => c.Id == course.courseID)))
            {
                ViewBag.error = "the student " + course.studentID.ToString() + " got class in this hour";
                ViewBag.courses = db.Course.Where(c => c.Id == course.courseID).ToList();
                List<User> students = new List<User>();
                List<scLink> scLinkList = scdb.scLink.ToList();
                foreach (var a in sdb.User.ToList())
                {
                    if (!scLinkList.Any(x => x.studentID == a.StudentID && x.courseID == course.courseID) && a.Rank < 2)
                    {
                        students.Add(a);
                    }
                }
                if (students.Count == 0)
                {
                    students = new List<User>();
                }
                ViewBag.students = students;
                return View(course);
            }

            if (ModelState.IsValid)
            {
                scdb.scLink.Add(course);
                gdb.Grades.Add(grade);
                scdb.SaveChanges();
                gdb.SaveChanges();
                return RedirectToAction("Index");
            }

            if (course.studentID == 0 || course.courseID == 0)
            {
                return RedirectToAction("Index");
            }
            return View(course);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveFromCourse([Bind(Include = "studentID, courseID")] scLink course)
        {
            if(course.studentID == 0 || course.courseID == 0)
            {
                return RedirectToAction("Index");
            }
     
            gdb.Grades.Remove(gdb.Grades.SingleOrDefault(u => u.studentID == course.studentID && u.courseID == course.courseID));
            scdb.scLink.Remove(scdb.scLink.SingleOrDefault(u => u.studentID == course.studentID && u.courseID == course.courseID));
            scdb.SaveChanges();
            gdb.SaveChanges();
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
