using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Web_omlate.DAL;
using Web_omlate.Models;

namespace Web_omlate.Controllers
{
    public class InstructorController : Controller
    {
        FYPDBContext _db = new FYPDBContext();
        // GET: Instructor
        public ActionResult Index()
        {
            object username = Session["username"];
            if (username != null)
            {
                string name = username.ToString();
                var user = _db.Users.
                    Where(x => x.Username == name).FirstOrDefault();
                ViewBag.offeredCourses = _db.OfferedCourses.Where(x => x.OfferdBy.Username == name && x.FinishDate >= DateTime.Now).ToList();

                return View(user);
            }
            //write something in temp message "please login first."
            return RedirectToAction("Index", "Default");
        }

        public ActionResult OfferACourse()
        {
            object username = Session["username"];
            if (username != null)
            {
                ViewBag.courseCategories = new SelectList(_db.CourseCategories, "CourseCategoryID", "CategoryName");
                //for the time sake only
                ViewBag.instructor = _db.Users.Where(x => x.Type == "instructor").Select(y => y.Username).FirstOrDefault();
                return View();
            }
            //write something in temp message "please login first."
            return RedirectToAction("Index", "Default");
        }

        public JsonResult CheckCourseCode(Course Course)
        {
            if (_db.OfferedCourses.Where(s => s.OfferdBy.Username == Session["username"].ToString() && s.Course.CourseCode.Equals(Course.CourseCode, StringComparison.CurrentCultureIgnoreCase) && s.FinishDate >= DateTime.Now).FirstOrDefault() == null)
                return Json(true, JsonRequestBehavior.AllowGet);
            return Json(false, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult OfferACourse([Bind(Include = "CourseCategoryID,OfferedByID,CourseCode,CourseTitle,CreditHours,StartingDate,FinishDate")] OfferedCourse offeredCourse)
        {
            if (ModelState.IsValid)
            {
                object username = Session["username"];
                if (username != null)
                {
                    var user = _db.Users.Where(x => x.Username == username.ToString()).FirstOrDefault();
                    if (user != null)
                    {
                        var cr = Request.Form["Course.CourseCode"];
                        var course = _db.OfferedCourses.Where(s => s.OfferdBy.Username == user.Username && s.FinishDate >= DateTime.Now && s.Course.CourseCode.Equals(cr, StringComparison.CurrentCultureIgnoreCase)).Select(s => s.Course).FirstOrDefault();
                        var c = _db.Courses.Where(s => s.CourseCode.Equals(cr, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                        var cc = int.Parse(Request.Form["Course.CourseCategoryID"]);
                        var category = _db.CourseCategories.Where(x => x.CourseCategoryID == cc).FirstOrDefault();

                        //if new course
                        if (course == null)
                        {
                            if (c == null)
                            {
                                course = new Course
                                {
                                    CourseCategory = category,
                                    CourseCategoryID = category.CourseCategoryID,
                                    CourseCode = cr,
                                    CourseTitle = Request.Form["Course.CourseTitle"]
                                };
                                c = course;
                                _db.Courses.Add(course);
                            }

                            offeredCourse.Course = c;
                            offeredCourse.OfferdBy = user;
                            offeredCourse.OfferedByID = user.Username;
                            offeredCourse.LearnerCount = 0;
                            _db.OfferedCourses.Add(offeredCourse);
                            _db.SaveChanges();

                            ViewBag.courseCategories = new SelectList(_db.CourseCategories, "CourseCategoryID", "CategoryName");
                            ViewBag.instructor = _db.Users.Where(x => x.Type == "instructor").Select(y => y.Username).FirstOrDefault();
                            ViewBag.msg = "Successfully Offered said course";
                            return OfferACourse();
                        }
                        ViewBag.msg = "You have already Offered said course";
                        ViewBag.courseCategories = new SelectList(_db.CourseCategories, "CourseCategoryID", "CategoryName");
                        ViewBag.instructor = _db.Users.Where(x => x.Type == "instructor").Select(y => y.Username).FirstOrDefault();
                        return View();
                    }
                    return RedirectToAction("Index");
                }
            }
            //write something in temp message "please login first."
            return RedirectToAction("Index", "Default");
        }

        public string CalculateMD5Hash(string input)
        {
            MD5 m = MD5.Create();
            byte[] inBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = m.ComputeHash(inBytes);
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                builder.Append(hashBytes[i].ToString("X2"));
            }
            return builder.ToString();
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
                var pass = CalculateMD5Hash(model.Password);
                var user = _db.Users.Where(x => x.Username == model.Username).FirstOrDefault();
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Field = model.Field;
                user.Password = pass;
                _db.SaveChanges();
                return RedirectToAction("Index", "Instructor");
            }
            //write something in temp message "please login first."
            return RedirectToAction("Index", "Default");
        }

        public ActionResult ScheduleLecture()
        {
            var name = Session["username"];
            if (name != null)
            {
                ViewBag.course = _db.OfferedCourses.Where(c => c.OfferdBy.Username == name.ToString() && c.FinishDate >= DateTime.Now).Select(x => new SelectListItem
                {
                    Value = x.OfferedCourseID.ToString(),
                    Text = (x.Course.CourseCode + " - " + x.Course.CourseTitle)
                }).ToList();
                return View();
            }
            //write something in temp message "please login first."
            return RedirectToAction("Index", "Default");

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ScheduleLecture([Bind(Include = "LectureDate,LectureTime,OfferedCourseID")] LectureSchedule model)
        {
            var name = Session["username"];
            if (name != null)
            {

                var course = _db.OfferedCourses.Where(x => x.OfferedCourseID == model.OfferedCourseID).FirstOrDefault();

                var code = course.Course.CourseCode;
                var date = model.LectureDate.ToString("yyyyMMdd");
                DateTime dt = new DateTime(2012, 01, 01);
                dt = dt + model.LectureTime;
                var time = dt.ToString("HHmmss");
                var xx = new LectureSchedule
                {
                    LectureTime = model.LectureTime,
                    OfferedCourseID = model.OfferedCourseID,
                    StreamName = code + date + time,
                    LectureDate = model.LectureDate,
                    OfferedCourse = course
                };
                _db.LectureSchedules.Add(xx);
                _db.SaveChanges();
                return RedirectToAction("ScheduledLectures");
            }
            //write something in temp message "please login first."
            return RedirectToAction("Index", "Default");

        }

        public ActionResult ScheduledLectures()
        {
            var name = Session["username"];
            if (name != null)
            {
                var dt = DateTime.Now.Date;
                var schedules = _db.LectureSchedules.Where(x => x.OfferedCourse.OfferdBy.Username == name.ToString() && x.LectureDate >= dt).OrderBy(s=>  new { s.LectureDate, s.LectureTime}).ToList();
                var toDel = schedules.Where(s => s.LectureDate == dt && s.LectureTime < DateTime.Now.TimeOfDay).ToList();
                foreach (var item in toDel)
                {
                    schedules.Remove(item);
                }
                return View(schedules);
            }
            //write something in temp message "please login first."
            return RedirectToAction("Index", "Default");
        }

        public JsonResult MyScheduledLectures(string email)
        {
            var dt = DateTime.Now.Date;
            var schedules = _db.LectureSchedules.Where(x => x.OfferedCourse.OfferdBy.Email.Equals(email, StringComparison.CurrentCultureIgnoreCase) && x.LectureDate >= dt).OrderBy(s=>s.LectureDate).ToList();
            TimeSpan baseTimeSpan = new TimeSpan(0, 1, 0, 0);
            var toDel = schedules.Where(s => s.LectureDate == dt && s.LectureTime < DateTime.Now.TimeOfDay.Subtract(baseTimeSpan) ).ToList();
            foreach (var item in toDel)
            {
                schedules.Remove(item);
            }
            return Json(schedules.Select(w => new
            {
                CourseCode = w.OfferedCourse.Course.CourseCode,
                CourseTitle = w.OfferedCourse.Course.CourseTitle,
                LectureDate = w.LectureDate.ToString("MM/dd/yyyy"),
                LectureTime = w.LectureTime.ToString()
            }).ToList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult ViewResources(int lectureId)
        {
            var resource = _db.LectureResources.Where(x => x.LectureScheduleID == lectureId).ToList();
            ViewBag.LectureScheduleID = lectureId;
            return View(resource);
        }

        public ActionResult UploadResource(int lectureid)
        {
            ViewBag.lectureid = lectureid;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadResource(int LectureScheduleID, string FileName, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (FileName == "")
                {
                    FileName = file.FileName;
                }
                var lec = _db.LectureSchedules.Where(s => s.LectureScheduleID == LectureScheduleID).FirstOrDefault();
                if (file != null && lec != null)
                {
                    var arr = new BinaryReader(file.InputStream);
                    
                    var data = arr.ReadBytes(file.ContentLength);
                    var fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName);
                    file.SaveAs(path);

                    _db.LectureResources.Add(new LectureResource
                    {
                        DateTime = DateTime.Now,
                        FilePath = path,
                        FileName = FileName,
                        LectureScheduleID = LectureScheduleID,
                        LectureSchedule = lec,
                        ResourceType = file.ContentType
                    });
                    _db.SaveChanges();
                    return RedirectToAction("ViewResources", new { lectureId = LectureScheduleID });
                }
                return View("ScheduledLectures");
            }
            return View("ScheduledLectures");
        }
        public ActionResult DownloadFile(int id, int lectureid)
        {
            var file = _db.LectureResources.Where(s => s.LectureResourceID == id).Select(d => new { Name = d.FileName, file = d.FilePath, Type = d.ResourceType }).FirstOrDefault();
            if (file != null)
            {
                return File(file.file, file.Type);
            }
            return RedirectToAction("ViewResources", new { lectureId = lectureid });
        }

        public ActionResult DeleteFile(int id, int lectureid)
        {
            var file = _db.LectureResources.Where(s => s.LectureResourceID == id).FirstOrDefault();
            if (file != null)
            {
                _db.LectureResources.Remove(file);
                _db.SaveChanges();
                return RedirectToAction("ViewResources", new { lectureId = lectureid });
            }
            return RedirectToAction("ViewResources", new { lectureId = lectureid });
        }

        public ActionResult ViewCourseDetails(int offeredCourseId)
        {
            var course = _db.LectureSchedules.Where(s => s.OfferedCourseID == offeredCourseId).Select(w => new
            {
                w.OfferedCourse.Course,
                w.OfferedCourse.Assessments,
                w.OfferedCourse,
                w.LectureResources
            }).FirstOrDefault();
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

        public ActionResult UploadAssessment(int courseId)
        {
            ViewBag.courseId = courseId;
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult UploadAssessment(int courseId, HttpPostedFileBase file, string AssessmentTitle, string note, int points, DateTime DueDate)
        {
            if (ModelState.IsValid)
            {
                var course = _db.OfferedCourses.Where(s => s.OfferedCourseID == courseId).FirstOrDefault();
                if (course != null)
                {
                    if (file != null)
                    {
                        var f = new BinaryReader(file.InputStream);

                        var data = f.ReadBytes(file.ContentLength);
                        var fileName = Path.GetFileName(file.FileName);
                        var path = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName);
                        file.SaveAs(path);

                        var assessment = new Assessment
                        {
                            AssessmentTitle = AssessmentTitle,
                            DateTime = DateTime.Now,
                            FilePath = path,
                            Note = note,
                            Points = points,
                            OfferedCourse = course,
                            FileType = file.ContentType,
                            DueDate = DueDate
                        };
                        _db.Assessments.Add(assessment);
                        _db.SaveChanges();
                        return RedirectToAction("ViewCourseDetails", new { offeredCourseId = courseId });
                    };
                }
            }
            return RedirectToAction("ViewCourseDetails", new { offeredCourseId = courseId });
        }

        public ActionResult DownloadAssessment(int id, int courseid)
        {
            var file = _db.Assessments.Where(s => s.AssessmentID == id).Select(d => new { Name = d.AssessmentTitle, file = d.FilePath, Type = d.FileType }).FirstOrDefault();
            if (file != null)
            {
                return File(file.file, file.Type);
            }
            return RedirectToAction("ViewCourseDetails", new { offeredCourseId = courseid });
        }
        public ActionResult DeleteAssessment(int id, int courseid)
        {

            var file = _db.Assessments.Where(s => s.AssessmentID == id).FirstOrDefault();
            if (file != null)
            {
                _db.Assessments.Remove(file);
                _db.SaveChanges();
                return RedirectToAction("ViewCourseDetails", new { offeredCourseId = courseid });
            }
            return RedirectToAction("ViewCourseDetails", new { offeredCourseId = courseid });
        }

        public ActionResult ViewSubmissions(int id, int courseid)
        {
            var submission = _db.AssessmentSubmissions.Where(s => s.AssessmentId == id).Select(w => new
            ViewSubmissionViewModel
            {
                SubmissionId = w.Id,
                SubmissionTime = w.SubmissionTime,
                LearnerName = _db.LearnerEnrollments.Where(s => s.LearnerEnrollID == w.LearnerId).Select(x => x.EnrolledLearner.Username).FirstOrDefault()
            }).ToList();
            return View(submission);
        }

        public ActionResult DownloadSubmission(int id)
        {
            var file = _db.AssessmentSubmissions.Where(s => s.Id == id).Select(s => new { Name = s.Assessment.AssessmentTitle, File = s.FilePath, Type = s.FileType }).FirstOrDefault();
            return File(file.File, file.Type);
        }

        public ActionResult CreateQuiz()
        {
            var name = Session["username"];
            if (name != null)
            {
                ViewBag.course = _db.OfferedCourses.Where(c => c.OfferdBy.Username == name.ToString() && c.FinishDate >= DateTime.Now).Select(x => new SelectListItem
                {
                    Value = x.OfferedCourseID.ToString(),
                    Text = (x.Course.CourseCode + " - " + x.Course.CourseTitle)
                }).ToList();
                var u = _db.Users.Where(x => x.Type == "instructor" && x.Username == (String)name).Select(y => y.Username).FirstOrDefault();
                ViewBag.instructor = u;
                return View();
            }
            return RedirectToAction("Index", "Default");
        }

        [HttpPost]
        public ActionResult CreateQuiz(Quiz quiz)
        {
            var name = Session["username"];
            if (name != null)
            {
                ViewBag.course = _db.OfferedCourses.Where(c => c.OfferdBy.Username == name.ToString() && c.FinishDate >= DateTime.Now).Select(x => new SelectListItem
                {
                    Value = x.OfferedCourseID.ToString(),
                    Text = (x.Course.CourseCode + " - " + x.Course.CourseTitle)
                }).ToList();

                ViewBag.instructor = _db.Users.Where(x => x.Type == "instructor" && x.Username == (String)name).Select(y => y.Username).FirstOrDefault();
                
                _db.Quizs.Add(quiz);
                _db.SaveChanges();
                int quizId = _db.Quizs.Max(item => item.QuizID);
                return RedirectToAction("EditQuiz", "Instructor", new { quizId });
            }
            return RedirectToAction("Index", "Default");
        }

        public ActionResult EditQuiz()
        {
            var name = Session["username"];
            if (name != null)
            {
                int quizId = int.Parse(Request["quizId"]);
                ViewBag.quizId = quizId;
                ViewBag.questions = _db.Quizs.Where(quiz => quiz.QuizID == quizId).Select(w => w.Questions).FirstOrDefault();
                ViewBag.attempts = _db.Quizs.Where(quiz => quiz.QuizID == quizId).Select(w => w.QuizAttempts).FirstOrDefault();
                return View();
            }
            return RedirectToAction("Index", "Default");
        }

        public ActionResult AddQuestionToQuiz()
        {
            var name = Session["username"];
            ViewBag.defaults = "def";
            if (name != null)
            {
                ViewBag.qid = Request["quizId"];
                return View();
            }
            return RedirectToAction("Index", "Default");
        }

        [HttpPost]
        public ActionResult AddQuestionToQuiz(QuizQuestion quizQuestion)
        {
            var name = Session["username"];
            if (name != null)
            {
                _db.QuizQuestions.Add(quizQuestion);
                _db.SaveChanges();
                int quizId = quizQuestion.QuizID;
                return RedirectToAction("EditQuiz", "Instructor", new { quizId });
            }
            return RedirectToAction("Index", "Default");
        }

        public ActionResult RemoveQuestion()
        {
            var name = Session["username"];
            int quizId = int.Parse(Request["quizId"]);
            if (name != null)
            {
                int qid = int.Parse(Request["qid"]);
                var t = _db.QuizQuestions.Where(q => q.ID == qid).FirstOrDefault();
                if(t!=null)
                {
                    _db.QuizQuestions.Remove(t);
                    _db.SaveChanges();
                    TempData["msg"] = "Question Successfully Deleted!";
                } else
                {
                    TempData["msg"] = "Question Not Found";
                }
                return RedirectToAction("EditQuiz", "Instructor", new { quizId });
            }
            return RedirectToAction("Index", "Default");
        }

        public ActionResult ViewQuiz()
        {
            var name = Session["username"];
            if (name != null)
            {
                ViewBag.quizs = _db.Quizs.Where(q => q.InstructorID == (String)name).OrderByDescending(s=>s.Deadline).ToList();
                ViewBag.offeredCourses = _db.OfferedCourses.Where(q => q.OfferedByID == (String)name).OrderByDescending(s=>s.FinishDate).ToList();
                //ViewBag.title = ;
                return View();
            }
            return RedirectToAction("Index", "Default");
        }
        public JsonResult MyCourses()
        {
            var instructorID = Request["instructorID"];
            var course = _db.OfferedCourses.Where(c => c.OfferdBy.Username == instructorID.ToString() && c.FinishDate >= DateTime.Now).Select(x => new
            {
                CourseCode = x.Course.CourseCode,
                CourseTitle = x.Course.CourseTitle,
                OfferedCourseID = x.OfferedCourseID
            }).ToList();
            return Json(course, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getQuizsbyOfferedCourseID()
        {
            var OfferedCourseID = Request["OfferedCourseID"];
            int qu = int.Parse(OfferedCourseID);
            var quizs = _db.Quizs.Where(q => q.offeredCourseID == qu & q.Deadline > DateTime.Now).Select(s => new
            {
                quizId = s.QuizID,
                quizTitle = s.QuizTitle,
                quizDuration = s.Duration
            }).ToList();
            return Json(quizs, JsonRequestBehavior.AllowGet);
        }

        public JsonResult validateUsernamePassword()
        {
            var username = Request["username"];

            var password = CalculateMD5Hash((String)Request["password"]);
            var u2 = _db.Users.Where(u => u.Username == username && u.Password == password && u.Type == "instructor").FirstOrDefault();
            if(u2!=null)
                return Json(true, JsonRequestBehavior.AllowGet);
            return Json(false, JsonRequestBehavior.AllowGet);
        }
    }

}