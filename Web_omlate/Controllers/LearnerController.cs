using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_omlate.DAL;
using Web_omlate.Models;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace Web_omlate.Controllers
{
    public class LearnerController : Controller
    {
        private FYPDBContext _db = new FYPDBContext();
        string username;
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
            return RedirectToAction("Index", "Default");
        }

        public ActionResult ViewOfferedCourses()
        {
            var username = Session["username"];
            if (username != null)
            {
                List<OfferedCoursesViewModel> courses = _db.OfferedCourses.Where(s=>s.FinishDate >=DateTime.Now).OrderByDescending(c=>c.LearnerCount).Select(x =>
                    new OfferedCoursesViewModel
                    {
                        OfferedCourseID = x.OfferedCourseID,
                        OfferedByID = x.OfferedByID,
                        Course = x.Course
                    }
                 ).ToList();
                List<LearnerEnroll> enrolls = _db.LearnerEnrollments.Where(s => s.EnrolledLearnerID == username.ToString()).ToList();
                foreach(LearnerEnroll enroll in enrolls)
                {
                    int courseId = _db.OfferedCourses.Where(s => s.OfferedCourseID == enroll.EnrolledCourseID).Select(x => x.OfferedCourseID).FirstOrDefault();
                    courses.Remove(courses.Where(s=>s.OfferedCourseID == courseId).FirstOrDefault());
                }
                ViewBag.Suggested = GetSuggestedCourses(username.ToString());
                return View(courses);
            }
            //TempData["msg"] = "Please Login or SignUp";
            return RedirectToAction("Index", "Default");
        }

        public ActionResult ViewCourseDetails(int offeredCourseId)
        {
            username = (String)Session["username"];
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
            var enroll = _db.LearnerEnrollments.Where(s => s.EnrolledLearnerID == username && s.EnrolledCourseID == offeredCourseId).FirstOrDefault();
            if(enroll!=null)
            {
                return RedirectToAction("ViewDetails", "Learner", new { offeredCourseId = offeredCourseId });
            }
            return View(courseDetails);
        }

        public ActionResult ViewDetails(int offeredCourseId)
        {
            username = (String)Session["username"];
            var course = _db.LectureSchedules.Where(s => s.OfferedCourseID == offeredCourseId).Select(w => new
            {
                w.OfferedCourse.Course,
                w.OfferedCourse,
                w.LectureResources,
                w.OfferedCourse.Assessments
            }).FirstOrDefault();
            var quizs = _db.Quizs.Where(q => q.offeredCourseID == offeredCourseId).ToList();
            List<double> marks = new List<double>();
            QuizAttempt attempt;
            foreach (var q in quizs )
            {
                attempt = _db.QuizAttempts.Where(at => at.LearnerID == username && at.QuizID == q.QuizID).FirstOrDefault();
                if (attempt == null)
                    marks.Add(-10000);
                else 
                    marks.Add(attempt.Marks);
                attempt = null;
            }
            //q2.QuizAttempts.Where(qs => qs.LearnerID == (String)username).FirstOrDefault();
            
            ViewBag.quizs = quizs;
            ViewBag.marks = marks;
            ViewBag.username = username;
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
                return File(file.file, file.Type);
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
                //var ax = course.CoursesEnrolled.Contains(_db.LearnerEnrollments.Where(s => s.EnrolledLearner.Username == name.ToString()).FirstOrDefault());
                var ax = _db.LearnerEnrollments.Where(s => s.EnrolledLearner.Username == name.ToString() && s.EnrolledCourseID == id).FirstOrDefault();
                if (ax ==null)
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
                var pass = CalculateMD5Hash(model.Password);
                var user = _db.Users.Where(x => x.Username == model.Username).FirstOrDefault();
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Field = model.Field;
                user.Password = pass;
                _db.SaveChanges();
                return RedirectToAction("Index", "Learner");
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

        public ActionResult ScheduledLectures()
        {
            var name = Session["username"];
            if (name != null)
            {
                var date = DateTime.Now.Date;
                var time = DateTime.Now.TimeOfDay;
                var schedules = _db.LectureSchedules.Where(s=>s.LectureDate>=date).OrderBy(s=>new
                {
                    s.LectureDate,
                    s.LectureTime
                }).ToList();

                var toDel = schedules.Where(s=> s.LectureDate == date && s.LectureTime<time).ToList();
                foreach (var item in toDel)
                {
                    schedules.Remove(item);
                }
                var learnerEnrolls = _db.LearnerEnrollments.Where(s=>s.EnrolledLearner.Username==name.ToString()).ToList();
                List<LectureSchedule> lectures = new List<LectureSchedule>();
                foreach(LearnerEnroll learnerEnroll in learnerEnrolls)
                {
                    lectures.AddRange(schedules.Where(s => s.OfferedCourse.CoursesEnrolled.Contains(learnerEnroll)).ToList());
                    
                }
                return View(lectures);
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
                return File(file.file, file.Type);
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
                var user = _db.Users.Where(s => s.Username == name.ToString()).FirstOrDefault();
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
                        Learner = user,
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
            var name = Session["username"];
            if (name != null)
            {
                var quiz = Request["quizId"];
                int qu = int.Parse(quiz);
                Quiz quizdetails = _db.Quizs.Where(q2 => q2.QuizID == qu).FirstOrDefault();

                if(_db.IsAttempteds.Where(a=>a.Username == (String)name && a.QuizID == quizdetails.QuizID).ToList().Count < 1)
                {
                    IsAttempted iA = new IsAttempted();
                    iA.AttemptedTime = DateTime.Now;
                    iA.Learner = _db.Users.Where(u => u.Username == (String)name).FirstOrDefault();
                    iA.Quiz = quizdetails;
                    _db.IsAttempteds.Add(iA);
                    _db.SaveChanges();
                } else
                {
                    TempData["msg"] = "You have Attempted this Quiz Already!";
                    var offeredCourseId = quizdetails.offeredCourseID;
                    return RedirectToAction("ViewDetails", "Learner", new { offeredCourseId });
                }


                ViewBag.quizId = quiz;
                ViewBag.OfferedCourseID = quizdetails.offeredCourseID;
                ViewBag.LearnerID = name;
                ViewBag.questions = quizdetails.Questions.ToList();
                ViewBag.duration = quizdetails.Duration * 60;

                return View();
            }
            return RedirectToAction("Index", "Default");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AttemptQuiz(QuizAttempt quizAttempt)
        {
            var name = Session["username"];
            if (name != null)
            {
                quizAttempt.Learner = _db.Users.Where(u => u.Username == (String)name).FirstOrDefault();
                quizAttempt.AttemptTime = DateTime.Now;
                quizAttempt.Quiz = _db.Quizs.Where(q => q.QuizID == quizAttempt.QuizID).FirstOrDefault();
                quizAttempt.OfferedCourse = _db.OfferedCourses.Where(o => o.OfferedCourseID == quizAttempt.OfferedCourseID).FirstOrDefault();
                var arr = quizAttempt.Answers.Split(new String[] { "!#!#!" }, StringSplitOptions.RemoveEmptyEntries);
                var questions = quizAttempt.Quiz.Questions.ToList();
                var i = 0;
                quizAttempt.Marks = 0;
                foreach(var q in questions)
                {
                    if (q.Answer == arr[i])
                    {
                        quizAttempt.Marks++;
                    }
                        i++;
                }

                _db.QuizAttempts.Add(quizAttempt);
                _db.SaveChanges();
                var offeredCourseId = quizAttempt.OfferedCourseID;
                TempData["msg"] = "Quiz Submitted Successfully";
                return RedirectToAction("ViewDetails", "Learner", new { offeredCourseId });
            }
            return RedirectToAction("Index", "Default");
        }


        public List<OfferedCourse> GetSuggestedCourses(String Username)
        {
            List<OfferedCourse> SuggestedCourses = new List<OfferedCourse>();
            List<LearnerEnroll> enrolls = _db.LearnerEnrollments.Where(s => s.EnrolledLearnerID == Username.ToString()).ToList();
            List<int> enrolledCourses = new List<int>();
            foreach (LearnerEnroll le in enrolls)
            {
                enrolledCourses.Add(le.EnrolledCourseID);
            }

            var rules = _db.Rules.OrderByDescending(s => s.Confidence).ToList();
            foreach(Rule r in rules)
            {
                List<int> Drivers = JsonConvert.DeserializeObject<List<int>>(r.Drivers);
                var firstNotSecond = enrolledCourses.Except(Drivers).ToList();
                var secondNotFirst = Drivers.Except(enrolledCourses).ToList();

                if (!firstNotSecond.Any() && !secondNotFirst.Any())
                {
                    List<int> Indicates = JsonConvert.DeserializeObject<List<int>>(r.Indicates);
                    foreach(int i in Indicates)
                    {
                        OfferedCourse oc = _db.OfferedCourses.Where(s => s.OfferedCourseID == i).FirstOrDefault();
                        if(oc.FinishDate >= DateTime.Now)
                        {
                            SuggestedCourses.Add(oc);
                        }
                    }
                }
            }

            //if (SuggestedCourses.Count < count)
            //{
            //    int remaining = count - SuggestedCourses.Count;
            //    var allOfferedCourses = _db.OfferedCourses.Where(s => s.FinishDate >= DateTime.Now).OrderByDescending(d => d.LearnerCount).ToList();
            //    int total = allOfferedCourses.Count;
            //    int i = 0;
            //    while(remaining>0 && i<total)
            //    {
            //        OfferedCourse ocTemp = allOfferedCourses.ElementAt(i);
            //        if(SuggestedCourses.Where(c=>c.OfferedCourseID == ocTemp.OfferedCourseID).FirstOrDefault()==null && !enrolledCourses.Contains(ocTemp.OfferedCourseID))
            //        {
            //            remaining--;
            //            SuggestedCourses.Add(ocTemp);
            //        }
            //        i++;
            //    }
            //}

            return SuggestedCourses;
        }
    }
}