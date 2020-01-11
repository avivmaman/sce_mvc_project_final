using MVC.Dal;
using MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC.Controllers
{
    public class newGradesIndex
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
        public int? aGrade { get; set; }
        public int? bGrade { get; set; }
    }

    public class HomeController : Controller
    {
        scLinkDal scdb = new scLinkDal();
        CourseDal cdb = new CourseDal();
        GradeDal gdb = new GradeDal();
        UserDal udb = new UserDal();

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index");
        }

        public ActionResult Index()
        {
            var a = Session["user"] as User;
            if(a != null)
            {
                ViewBag.user = a;
                List<newGradesIndex> courses = new List<newGradesIndex>();

                if (a.Rank < 2)
                {
                    foreach (scLink link in scdb.scLink.Where(u => u.studentID == a.StudentID).ToList())
                    {
                        foreach (Course b in cdb.Course.AsNoTracking().OrderByDescending(o => o.day).ToList())
                        {
                            if (link.courseID == b.Id)
                            {
                                newGradesIndex temp = new newGradesIndex();
                                temp.Id = b.Id;
                                temp.startTime = b.startTime;
                                temp.duration = b.duration;
                                temp.teacher = udb.User.FirstOrDefault(u => u.StudentID == b.teacher).FullName;
                                temp.name = b.name;
                                temp.classroom = b.classroom;
                                temp.classrooma = b.classrooma;
                                temp.classroomb = b.classroomb;
                                temp.day = b.day;
                                temp.aDate = b.aDate;
                                temp.bDate = b.bDate;
                                temp.aGrade = gdb.Grades.FirstOrDefault(s => s.studentID == a.StudentID && s.courseID == b.Id).aGrade;
                                temp.bGrade = gdb.Grades.FirstOrDefault(s => s.studentID == a.StudentID && s.courseID == b.Id).bGrade;
                                courses.Add(temp);
                            }
                        }
                    }
                }
                if (a.Rank == 2)
                {
                        foreach (Course b in cdb.Course.AsNoTracking().Where(u => u.teacher == a.StudentID).OrderByDescending(o => o.day).ToList())
                        {
                            newGradesIndex temp = new newGradesIndex();
                            temp.Id = b.Id;
                            temp.startTime = b.startTime;
                            temp.duration = b.duration;
                            temp.teacher = udb.User.FirstOrDefault(u => u.StudentID == b.teacher).FullName;
                            temp.name = b.name;
                            temp.classroom = b.classroom;
                            temp.classrooma = b.classrooma;
                            temp.classroomb = b.classroomb;
                            temp.day = b.day;
                            temp.aDate = b.aDate;
                            temp.bDate = b.bDate;
                            temp.aGrade = 0;
                            temp.bGrade = 0;
                            courses.Add(temp);
                        }
                   
                }
                List<newGradesIndex> courses2 = new List<newGradesIndex>(courses);
                courses2.Sort((x, y) => x.aDate.CompareTo(y.aDate));
                ViewBag.courses = courses;
                ViewBag.courses2 = courses2;
            }
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Login()
        {
            ViewBag.Message = "Your login page.";
            UserDal udal = new UserDal();
            User t = new User
            {
                StudentID = 123456789,
                Password = "123456",
                Email = "mmn.aviv@gmail.com",
                FullName = "Aviv Maman Admin",
                Rank = 3
            };
            var count = udal.User.Where(u => u.StudentID == t.StudentID).Count();
            if (ModelState.IsValid && count == 0)
            {
                udal.User.Add(t);
                udal.SaveChanges();
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(User model)
        {
            UserDal udal = new UserDal();
            List<User> t = udal.User.Where(u => u.StudentID == model.StudentID && u.Password == model.Password).ToList();
            if(t.Count > 0)
            {
                Session["user"] = t[0];
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.error = "Incorrect id or pass";
            }
            return View();
        }
    }
}