using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BigShool_1911065248.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace BigShool_1911065248.Controllers
{
    public class CoursesController : Controller
    {
        // GET: Courses
        BigShoolContext context = new BigShoolContext();
        public ActionResult Create()
        {
            BigShoolContext Context = new BigShoolContext();
            Course ObjCourse = new Course();
            ObjCourse.listCategory = Context.Categories.ToList();

            return View(ObjCourse);
        }
        [Authorize]
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create( Course objCourse)
        {
            BigShoolContext context = new BigShoolContext();


            //ModelState.Remove("LecturerId");
            //if(!ModelState.IsValid)
            //{
            //    objCourse.ListCategory = context.Categories.ToList();
            //    return View("Create", objCourse);
            //}

            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            objCourse.LecturerId = user.Id;

            context.Courses.Add(objCourse);
            context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }
        public ActionResult Attending()
        {
            BigShoolContext context = new BigShoolContext();
            ApplicationUser currenUser = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var listAttendances = context.Attendances.Where(p => p.Attendee == currenUser.Id).ToList();
            var courses = new List<Course>();
            foreach (Attendance temp in listAttendances)
            {
                Course objCourse = temp.Course;
                objCourse.LecturerId = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(objCourse.LecturerId).Name;
                courses.Add(objCourse);
            }
            return View(courses);
        }
        public ActionResult Mine()
        {
            BigShoolContext context = new BigShoolContext();
            ApplicationUser currentUser = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>()
                                .FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var courses = context.Courses.Where(c => c.LecturerId == currentUser.Id && c.DateTime > DateTime.Now).ToList();
            foreach (Course i in courses)
            {
                i.Name = currentUser.Name;
            }
            return View(courses);
        }

        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Course course = context.Courses.FirstOrDefault(p => p.Id == id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }
        [HttpPost]
        public ActionResult Delete(Course course)
        {
            Course obj = context.Courses.FirstOrDefault(p => p.Id == course.Id);
            if (obj != null)
            {
                context.Courses.Remove(obj);
                context.SaveChanges();
            }
            return RedirectToAction("Mine", "Course");
        }

        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Course course = context.Courses.FirstOrDefault(p => p.Id == id);
            if (course == null)
            {
                return HttpNotFound();
            }
            course.listCategory = context.Categories.ToList();
            return View(course);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Course course)
        {
            ModelState.Remove("LecturerID");
            if (ModelState.IsValid)
            {
                var edit = context.Courses.FirstOrDefault(e => e.Id == course.Id);
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
                edit.LecturerId = user.Id;
                edit.Place = course.Place;
                edit.DateTime = course.DateTime;
                edit.CategoryId = course.CategoryId;
                context.Courses.AddOrUpdate(edit);
                context.SaveChanges();
                return RedirectToAction("Mine", "Course");
            }
            else
            {
                ModelState.AddModelError("", "Input Model Not Valid");
                return View(course);
            }
        }
        public ActionResult LectureIamGoing()
        {
            ApplicationUser currentUser =
            System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>()
            .FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            BigShoolContext context = new BigShoolContext();
            //danh sách giảng viên được theo dõi bởi người dùng (đăng nhập) hiện tại
            var listFollwee = context.Followings.Where(p => p.FollowerId ==
            currentUser.Id).ToList();
            //danh sách các khóa học mà người dùng đã đăng ký
            var listAttendances = context.Attendances.Where(p => p.Attendee ==
            currentUser.Id).ToList();
            var courses = new List<Course>();
            foreach (var course in listAttendances)
            {
                foreach (var item in listFollwee)
                {
                    if (item.FolloweeId == course.Course.LecturerId)
                    {
                        Course objCourse = course.Course;
                        objCourse.LectureName =
                       System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>()
                        .FindById(objCourse.LecturerId).Name;
                        courses.Add(objCourse);
                    }
                }

            }
            return View(courses);
        }
    }

}