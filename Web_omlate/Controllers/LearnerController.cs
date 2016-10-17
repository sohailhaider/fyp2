﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_omlate.DAL;
using Web_omlate.Models;

namespace Web_omlate.Controllers
{
    public class LearnerController : Controller
    {
        private FYPDBContext _db = new FYPDBContext();
        // GET: Learner
        public ActionResult Index()
        {
            var username = Session["username"];
            if (username != null)
            {
                var name = username.ToString();
                var user = _db.Users.
                    Where(x => x.Username == name).FirstOrDefault();
                return View(user);
            }
            //write something in temp message "please login first."
            return RedirectToAction("Index", "Default");
        }
        public JsonResult EnrolledCourses(string email)
        {
            var courses = _db.LearnerEnrollments.Where(s => s.EnrolledLearner.Email.Equals(email, StringComparison.CurrentCultureIgnoreCase)).Select(s => s.EnrolledCourse).ToList();
            return Json(courses, JsonRequestBehavior.AllowGet);
        }

        public JsonResult MyScheduledLectures(string email)
        {
            var date = DateTime.Now.Date;
            var time = DateTime.Now.TimeOfDay;
            var schedules = _db.LectureSchedules.Where(s => s.LectureDate >= date).ToList();

            var toDel = schedules.Where(s => s.LectureTime < time).ToList();
            foreach (var item in toDel)
            {
                schedules.Remove(item);
            }
            var learner = _db.LearnerEnrollments.Where(s => s.EnrolledLearner.Email == email).FirstOrDefault();
            var hisSches = schedules.Where(s => s.OfferedCourse.CoursesEnrolled.Contains(learner)).Select(w => new {
                CourseCode = w.OfferedCourse.Course.CourseCode,
                CourseTitle = w.OfferedCourse.Course.CourseTitle,
                LectureDate = w.LectureDate.ToString("MM/dd/yyyy"),
                LectureTime = w.LectureTime.ToString()
            });
            return Json(hisSches, JsonRequestBehavior.AllowGet);
        }

        public string GetLectureString(string lecId)
        {
            return "class1";
        }
        public ActionResult EnrollACourse()
        {
            var username = Session["username"];
            if (username != null)
            {
                var offeredCourses = _db.OfferedCourses.ToList();

                return View(offeredCourses);
            }
            TempData["error_message"] = "Please Login or SignUp";
            return View("Index", "Default");
        }

        public ActionResult ViewOfferedCourses()
        {
            var username = Session["username"];
            if (username != null)
            {
                List<OfferedCoursesViewModel> courses = _db.OfferedCourses.Select(x =>
                    new OfferedCoursesViewModel
                    {
                        OfferedCourseID = x.OfferedCourseID,
                        OfferedByID = x.OfferedByID,
                        Course = x.Course
                    }
                 ).ToList();

                return View(courses);
            }
            TempData["error_message"] = "Please Login or SignUp";
            return View("Index", "Default");
        }

        public ActionResult ViewCourseDetails(int offeredCourseId)
        {
            var courseDetails = _db.OfferedCourses.Where(x => x.OfferedCourseID == offeredCourseId).
                Select(y => new
                EnrollCourseViewModel
                {
                    OfferedCourseID = y.OfferedCourseID,
                    StartingDate = y.StartingDate,
                    FinishDate = y.FinishDate,
                    InstructorName = y.OfferdBy.FirstName + " " + y.OfferdBy.LastName,
                    CourseCode = y.Course.CourseCode,
                    CourseTitle = y.Course.CourseTitle,
                    CategoryName = y.Course.CourseCategory.CategoryName
                }).FirstOrDefault();
            return View(courseDetails);
        }

        public ActionResult ViewDetails(int offeredCourseId)
        {
            var course = _db.LectureSchedules.Where(s => s.OfferedCourseID == offeredCourseId).Select(w => new
            {
                w.OfferedCourse.Course,
                w.OfferedCourse,
                w.LectureResources,
                w.OfferedCourse.Assessments
            }).FirstOrDefault();
            var quizs = _db.Quizs.Where(q => q.offeredCourseID == offeredCourseId).ToList();
            ViewBag.quizs = quizs;
            if (course != null)
            {
                return View(new CourseDetailsViewModel
                {
                    Course = course.Course,
                    LectureResources = course.LectureResources,
                    OfferedCourse = course.OfferedCourse,
                    Assessments = course.Assessments
                });
            }
            var c = _db.OfferedCourses.Where(s => s.OfferedCourseID == offeredCourseId).FirstOrDefault();
            return View(new CourseDetailsViewModel
            {
                Course = c.Course,
                LectureResources = new List<LectureResource>(),
                OfferedCourse = c,
                Assessments = new List<Assessment>()
            });
        }
        public ActionResult DownloadFile(int id, int lectureid)
        {
            var file = _db.LectureResources.Where(s => s.LectureResourceID == id).Select(d => new { Name = d.FileName, file = d.FilePath, Type = d.ResourceType }).FirstOrDefault();
            if (file != null)
            {
                return File(file.file, file.Type,file.Name);
            }
            return RedirectToAction("ViewResources", new { lectureId = lectureid });
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult SaveCourseEnroll([Bind(Include = "OfferedCourseID")]EnrollCourseViewModel model)
        {
            var name = Session["username"];
            if (name != null)
            {
                var id = model.OfferedCourseID;
                var course = _db.OfferedCourses.Where(x => x.OfferedCourseID == id).FirstOrDefault();
                var learner = _db.Users.Where(x => x.Username == name.ToString()).FirstOrDefault();
                var ax = course.CoursesEnrolled.Contains(_db.LearnerEnrollments.Where(s => s.EnrolledLearner.Username == name.ToString()).FirstOrDefault());
                if (ax != null)
                {
                    course.LearnerCount = course.LearnerCount + 1;
                    _db.LearnerEnrollments.Add(new LearnerEnroll
                    {
                        EnrollDate = DateTime.Now,
                        EnrolledCourse = course,
                        EnrolledLearner = learner,
                        EnrolledLearnerID = learner.Username,
                        CompletionDate = course.FinishDate,
                        EnrolledCourseID = id,
                    });

                    _db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index", "Default");
        }
        public ActionResult EditInfo(string username)
        {
            var name = Session["username"];

            if (name != null)
            {
                var user = _db.Users.Where(x => x.Username == username.ToString()).FirstOrDefault();
                ViewBag.fields = _db.CourseCategories.Select(x => new SelectListItem
                {
                    Text = x.CategoryName,
                    Value = x.CategoryName,
                    Selected = x.CategoryName == user.Field
                }).ToList();
                return View(user);
            }
            //write something in temp message "please login first."
            return RedirectToAction("Index", "Default");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveEditInfo([Bind(Include = "Username,FirstName,LastName,Field,Password")]User model)
        {
            var name = Session["username"];
            if (name != null)
            {
                var user = _db.Users.Where(x => x.Username == model.Username).FirstOrDefault();
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Field = model.Field;
                user.Password = model.Password;
                _db.SaveChanges();
                return RedirectToAction("Index", "Learner");
            }
            //write something in temp message "please login first."
            return RedirectToAction("Index", "Default");
        }

        public ActionResult ScheduledLectures()
        {
            var name = Session["username"];
            if (name != null)
            {
                var date = DateTime.Now.Date;
                var time = DateTime.Now.TimeOfDay;
                var schedules = _db.LectureSchedules.Where(s=>s.LectureDate>=date).ToList();

                var toDel = schedules.Where(s=>s.LectureTime<time).ToList();
                foreach (var item in toDel)
                {
                    schedules.Remove(item);
                }
                var learner=_db.LearnerEnrollments.Where(s=>s.EnrolledLearner.Username==name.ToString()).FirstOrDefault();
                var hisSches = schedules.Where(s => s.OfferedCourse.CoursesEnrolled.Contains(learner));
                return View(hisSches);
            }
            return RedirectToAction("Index", "Default");
        }

        public ActionResult TakeLecture(int LectureScheduleID)
        {
            var name = Session["username"];
            if (name != null)
            {
                var lecture = _db.LectureSchedules.Where(x => x.LectureScheduleID == LectureScheduleID).FirstOrDefault();
                if (lecture != null)
                {
                    return View(lecture);
                }
                return RedirectToAction("ScheduledLectures");
            }
            return RedirectToAction("Index", "Default");
        }

        public ActionResult DownloadAssessment(int id, int courseid)
        {
            var file = _db.Assessments.Where(s => s.AssessmentID == id).Select(d => new { Name = d.AssessmentTitle, file = d.FilePath, Type = d.FileType }).FirstOrDefault();
            if (file != null)
            {
                return File(file.file, file.Type,file.Name);
            }
            return RedirectToAction("ViewCourseDetails", new { offeredCourseId = courseid });
        }

        public ActionResult SubmitAssessment(int id, int courseId)
        {
            var assign = _db.Assessments.Where(s => s.AssessmentID == id && s.DateTime <= DateTime.Now).FirstOrDefault();
            if (assign != null)
            {
                ViewBag.AssessmentTitle = assign.AssessmentTitle;
                ViewBag.DueDate = assign.DueDate;
                ViewBag.Points = assign.Points;
                ViewBag.AssessmentID = id;
                ViewBag.courseId = courseId;
                return View();
            }
            return RedirectToAction("ViewCourseDetails", new { offeredCourseId = courseId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitAssessment(int AssessmentID, int courseId, HttpPostedFileBase File)
        {
            if (File != null)
            {
                var name = Session["username"];
                var learner = _db.LearnerEnrollments.Where(d => d.EnrolledLearner.Username == name.ToString()).FirstOrDefault();
                if (learner != null)
                {
                    var f = new BinaryReader(File.InputStream);
                    var data = f.ReadBytes(File.ContentLength);
                    var fileName = Path.GetFileName(File.FileName);
                    var path = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName);
                    File.SaveAs(path);

                    _db.AssessmentSubmissions.Add(new AssessmentSubmission
                    {
                        Assessment = _db.Assessments.Where(s => s.AssessmentID == AssessmentID).FirstOrDefault(),
                        AssessmentId = AssessmentID,
                        FilePath = path,
                        LearnerId = learner.LearnerEnrollID,
                        SubmissionTime = DateTime.Now,
                        FileType = File.ContentType
                    });
                    _db.SaveChanges();
                }
            }
            return RedirectToAction("ViewCourseDetails", new { offeredCourseId = courseId });
        }

        public ActionResult AttemptQuiz()
        {
            return View();
        }
    }
}