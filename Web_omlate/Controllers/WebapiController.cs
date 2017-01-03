using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_omlate.Models;
using Web_omlate.DAL;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Net.Http.Headers;
using System.Web.Http;
using Web_omlate.DAO;
using Web_omlate.ApiModels;
using System.Security.Cryptography;

namespace Web_omlate.Controllers
{
    public class WebapiController : Controller
    {
        FYPDBContext _db = new FYPDBContext();
        
        public JsonResult Login(UserLoginModel loginCredentials)
        {
            var pass = CalculateMD5Hash(loginCredentials.Password);
            var user =
                   _db.Users.Where(
                       s =>
                           s.Email.Equals(loginCredentials.Email, StringComparison.CurrentCultureIgnoreCase) && s.Password == pass &&
                           s.Type.ToLower() == "learner").Select(s => new
                           {
                               FirstName = s.FirstName,
                               LastName = s.LastName,
                               Field = s.Field,
                               Username = s.Username,
                               Email = s.Email,
                               PhoneNo = s.PhoneNo,
                               Type = s.Type
                           }).FirstOrDefault();
            if(user == null)
            {
                return Json(new ApiResponse
                {
                    Message = "Invalid Email/Password!",
                    Status = false,
                }, JsonRequestBehavior.AllowGet);

            }
            return Json(new ApiResponse
            {
                Message = "Successfull Login!",
                Status = true,
                Data = user
            }, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetActiveCoursesByInstructor(String instructorId)
        {
            var list = _db.OfferedCourses.Where(s => s.OfferedByID == instructorId && s.FinishDate >= DateTime.Now && s.StartingDate <= DateTime.Now).Select(s=>new { s.OfferedCourseID, s.Course.CourseTitle, s.Course.CourseCode}).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSchedulesByLearner(String Username)
        {
            if (Username != null)
            {
                var date = DateTime.Now.Date;
                var time = DateTime.Now.TimeOfDay;
                var schedules = _db.LectureSchedules.Where(s => s.LectureDate >= date ).OrderBy(s => new
                {
                    s.LectureDate,
                    s.LectureTime
                }).ToList();

                var toDel = schedules.Where(s => s.LectureDate == date && s.LectureTime < time).ToList();
                foreach (var item in toDel)
                {
                    schedules.Remove(item);
                }
                var learnerEnrolls = _db.LearnerEnrollments.Where(s => s.EnrolledLearner.Username == Username.ToString()).ToList();
                List<LectureSchedule> lectures = new List<LectureSchedule>();
                foreach (LearnerEnroll learnerEnroll in learnerEnrolls)
                {
                    lectures.AddRange(schedules.Where(s => s.OfferedCourse.CoursesEnrolled.Contains(learnerEnroll)).ToList());
                }
                lectures.Select(s => new
                {
                    LectureScheduleID = s.LectureScheduleID,
                });
                return Json(new ApiResponse
                {
                    Message = "Found",
                    Status = true,
                    Data = lectures.Select(s => new
                    {
                        s.LectureScheduleID,
                        s.OfferedCourseID,
                        Date = s.LectureDate.ToString("dd/MM/yyyy"),
                        s.LectureTime.Hours,
                        s.LectureTime.Minutes,
                        s.LectureTime.Seconds,
                        s.OfferedCourse.OfferedByID,
                        s.OfferedCourse.Course.CourseTitle
                    }),
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new ApiResponse
            {
                Message = "Invalid Username",
                Status = false,
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetScheduledByInstructor(String Username)
        {
            if (Username != null)
            {
                var date = DateTime.Now.Date;
                var time = DateTime.Now.TimeOfDay;
                var schedules = _db.LectureSchedules.Where(s => s.LectureDate >= date && s.LectureTime >= time && s.OfferedCourse.OfferedByID == Username).OrderBy(s => new
                {
                    s.LectureDate,
                    s.LectureTime
                }).ToList();
                
                return Json(schedules.Select(s => new
                    {
                        s.LectureScheduleID,
                        s.OfferedCourseID,
                        Date = s.LectureDate.ToString("dd/MM/yyyy"),
                        s.LectureTime.Hours,
                        s.LectureTime.Minutes,
                        s.LectureTime.Seconds,
                        s.OfferedCourse.OfferedByID,
                        s.OfferedCourse.Course.CourseTitle
                    }).ToList(), JsonRequestBehavior.AllowGet);
            }

            return Json(new ApiResponse
            {
                Message = "Invalid Username",
                Status = false,
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getAllCoursesWithLearnerId(string Username)
        {
            List<OfferedCourseViewModelInAPI> courses = _db.OfferedCourses.Where(s=>s.FinishDate >= DateTime.Now).Select(x =>
                    new OfferedCourseViewModelInAPI
                    {
                        OfferedCourseID = x.OfferedCourseID,
                        OfferedByID = x.OfferedByID,
                        StartDate = x.StartingDate,
                        CreditHours = x.CreditHours,
                        FinishDate = x.FinishDate,
                        Course = x.Course
                    }
                 ).ToList();
            List<LearnerEnroll> enrolls = _db.LearnerEnrollments.Where(s => s.EnrolledLearnerID == Username.ToString()).ToList();
            foreach (LearnerEnroll enroll in enrolls)
            {
                int courseId = _db.OfferedCourses.Where(s => s.OfferedCourseID == enroll.EnrolledCourseID).Select(x => x.OfferedCourseID).FirstOrDefault();
                courses.Remove(courses.Where(s => s.OfferedCourseID == courseId).FirstOrDefault());
            }
            if(courses.Count ==0)
            {
                return Json(new ApiResponse
                {
                    Message = "No Course Found",
                    Status = false,
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new ApiResponse
            {
                Message = "Found",
                Status = true,
                Data = courses.Select(s => new
                {
                    s.OfferedByID,
                    s.OfferedCourseID,
                    s.Course.CourseTitle,
                    s.Course.CourseCategoryID,
                    s.Course.CourseCode,
                    s.CreditHours,
                    StartDate = s.StartDate.ToString("dd/MM/yyyy"),
                    FinishDate = s.FinishDate.ToString("dd/MM/yyyy"),
                    s.Course.CourseImage
                }).ToList(),
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getEnrolledCoursesByLearnerId(string Username)
        {
            var user = _db.Users.
                    Where(x => x.Username == Username).FirstOrDefault();



            return Json(new ApiResponse
            {
                Message = "Found",
                Status = true,
                Data = user.EnrolledCourses.
                Select(s => new
                {
                    s.EnrolledCourse.OfferedByID,
                    s.EnrolledCourse.OfferedCourseID,
                    s.EnrolledCourse.CreditHours,
                    StartDate = s.EnrolledCourse.StartingDate.ToString("dd/MM/yyyy"),
                    FinishDate = s.EnrolledCourse.FinishDate.ToString("dd/MM/yyyy"),
                    s.EnrolledCourse.Course.CourseTitle,
                    s.EnrolledCourse.Course.CourseCategoryID,
                    s.EnrolledCourse.Course.CourseCode,
                    s.EnrolledCourse.Course.CourseImage
                }
                ).ToList(),
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getAssignmentsByLearnerId(string Username)
        {
            var user = _db.Users.
                    Where(x => x.Username == Username).FirstOrDefault();
            List<Assessment> lectures = new List<Assessment>();
            foreach(LearnerEnroll oc in user.EnrolledCourses)
            {
                lectures.AddRange(oc.EnrolledCourse.Assessments.OrderBy(d=>d.DueDate).ToList());
            }

            return Json(new ApiResponse
            {
                Message = "Found",
                Status = true,
                Data = lectures.
                Select(s => new
                {
                    s.AssessmentID,
                    s.AssessmentTitle,
                    DueDate = s.DueDate.ToString("dd/MM/yyyy"),
                    DateTime = s.DateTime.ToString("dd/MM/yyyy"),
                    s.Note,
                    s.Points,
                    s.FilePath,
                    s.FileType,
                    s.OfferedCourse.Course.CourseCode,
                    s.OfferedCourse.Course.CourseTitle,
                    s.OfferedCourse.OfferedByID,
                    Submissions = (s.AssessmentSubmissions == null) ? new List<SubimissionTempate>() : s.AssessmentSubmissions.Select(b => new SubimissionTempate { LearnerId = b.Learner.Username, SubmissionTime = b.SubmissionTime.ToString() }).ToList(),
                }
                ).ToList(),
            }, JsonRequestBehavior.AllowGet);

    }

        public JsonResult enrollCourse(EnrollCourseInAPI enroll)
        {
            var user = _db.Users.Where(s => s.Username == enroll.LearnerId).FirstOrDefault();
            if(user == null)
            {
                return Json(new ApiResponse
                {
                    Message = "Try Login then enroll",
                    Status = false,
                }, JsonRequestBehavior.AllowGet);
            }
            int id = Int32.Parse(enroll.OfferedCourseId);
            var course = _db.OfferedCourses.Where(x => x.OfferedCourseID == id).FirstOrDefault();
            if (course == null)
            {
                return Json(new ApiResponse
                {
                    Message = "Select Valid Course",
                    Status = false,
                }, JsonRequestBehavior.AllowGet);
            }
            var ax = _db.LearnerEnrollments.Where(s => s.EnrolledLearner.Username == enroll.LearnerId.ToString() && s.EnrolledCourseID == id).FirstOrDefault();
            if (ax == null)
            {
                course.LearnerCount = course.LearnerCount + 1;
                _db.LearnerEnrollments.Add(new LearnerEnroll
                {
                    EnrollDate = DateTime.Now,
                    EnrolledCourse = course,
                    EnrolledLearner = user,
                    EnrolledLearnerID = user.Username,
                    CompletionDate = course.FinishDate,
                    EnrolledCourseID = id,
                });

                _db.SaveChanges();
                
                return Json(new ApiResponse
                {
                    Message = "Course Enrolled",
                    Status = true,
                }, JsonRequestBehavior.AllowGet);
            }



            return Json(new ApiResponse
            {
                Message = "Invalid Information",
                Status = false,
            }, JsonRequestBehavior.AllowGet);
        }

        //public Object StartSession(session s)
        //{
        //    int inputPort = 1000;
        //    int outputPort = 2000;
        //    int index = 0; 

        //    while (FYPDBContext.inputPorts.Contains(inputPort + index))
        //        index++;
        //    inputPort += index;


        //    index = 0;
        //    while (FYPDBContext.outputPorts.Contains(outputPort + index))
        //        index++;
        //    outputPort += index;

        //    FYPDBContext.inputPorts.Add(inputPort);
        //    FYPDBContext.outputPorts.Add(outputPort);

        //    s.sessionInPort = inputPort;
        //    s.sessionOutPort = outputPort;
        //    //s.session_expected_time = 20;
        //       // s.sessionClientIp = Request.ToString();

        //    _db.Sessions.Add(s);
        //    _db.SaveChanges();

        //    return new { msg = "Session started", sessionDetails = s};
        //}

        //public Object EndSession(int id)
        //{
        //    //session sessionObj = new session() { sessionId = id };
        //    var sessionObj = _db.Sessions.Where(s => s.sessionId == id).FirstOrDefault();
        //    if(sessionObj == null)
        //    {
        //        return "No Session Exists with given details!";
        //    }

        //    FYPDBContext.inputPorts.Remove(sessionObj.sessionInPort);
        //    FYPDBContext.inputPorts.Remove(sessionObj.sessionOutPort);

        //    _db.Sessions.Attach(sessionObj);
        //    _db.Sessions.Remove(sessionObj);
        //    _db.SaveChanges();

        //    return "Session Ended";
        //}

        //public Object Login(User User)
        //{
        //    //User User = new DAO.User() { Email = "ammar@gmail.com", Password = "ammar" };
        //    var userDetails = _db.Users.Where(u => u.user_email == User.Email && u.user_password == User.Password).FirstOrDefault();
        //    if (userDetails == null)
        //        return new { msg = "Invalid User Credentials", UserDetails = -1 };

        //    return new { msg = "User Success", UserDetails = userDetails.userId };
        //}


        //public Object getIpPort(int id)
        //{
        //    var userDetails = _db.ClassSchedules.Where(u => u.instructorId == id && u.inSession !=0).FirstOrDefault();
        //    if (userDetails == null)
        //        return new { msg = "No scheduled session available", port = -1 };

        //    int sid =userDetails.inSession;
        //    var sessionDetails = _db.Sessions.Where(u => u.sessionId == sid).FirstOrDefault();

        //    return new { msg = "User Success", port = sessionDetails.sessionInPort };
        //}

        //public Object atest()
        //{
        //    return new { userid = "asds" };
        //}
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


        public JsonResult getQuizById(QuizGetViewModel quizGetter)
        {
            int id = Int32.Parse(quizGetter.id);
            Quiz quizdetails = _db.Quizs.Where(q2 => q2.QuizID == id).FirstOrDefault();
            if (_db.IsAttempteds.Where(a => a.Username == quizGetter.Username && a.QuizID == quizdetails.QuizID).ToList().Count < 1)
            {
                IsAttempted iA = new IsAttempted();
                iA.AttemptedTime = DateTime.Now;
                iA.Learner = _db.Users.Where(u => u.Username == quizGetter.Username).FirstOrDefault();
                iA.Quiz = quizdetails;
                _db.IsAttempteds.Add(iA);
                _db.SaveChanges();
            }
            else
            {
                return Json(new ApiResponse
                {
                    Message = "Quiz already attempted",
                    Status = false,
                }, JsonRequestBehavior.AllowGet);
            }


            return Json(new ApiResponse
            {
                Message = "Found Information",
                Status = true,
                Data = new
                {
                    quizdetails.Questions.Count,
                    Questions = quizdetails.Questions.Select(s=>new {
                        s.ID,
                        s.QuestionStatement,
                        s.Options,
                        s.Answer
                    }).ToList(),
                    quizdetails.Duration
                }
            }, JsonRequestBehavior.AllowGet);

            return Json(new ApiResponse
            {
                Message = "Invalid Information",
                Status = false,
            }, JsonRequestBehavior.AllowGet);

        }


        public JsonResult SaveQuizAttempt(QuizAttemptInModel qA)
        {
            int id = Int32.Parse(qA.QuizId);
            var Quiz = _db.Quizs.Where(s => s.QuizID == id).FirstOrDefault();
            var User = _db.Users.Where(s => s.Username == qA.LearnerId).FirstOrDefault();
            var offeredCourse = _db.OfferedCourses.Where(s => s.OfferedCourseID == Quiz.offeredCourseID).FirstOrDefault();

            if (Quiz == null)
            {
                return Json(new ApiResponse
                {
                    Message = "Quiz Not Found",
                    Status = false,
                }, JsonRequestBehavior.AllowGet);
            }
            if (User == null)
            {
                return Json(new ApiResponse
                {
                    Message = "User details not found",
                    Status = false,
                }, JsonRequestBehavior.AllowGet);
            }


            QuizAttempt QuizAmpt = new QuizAttempt();

            QuizAmpt.LearnerID = qA.LearnerId;
            QuizAmpt.OfferedCourseID = Quiz.offeredCourseID;
            QuizAmpt.AttemptTime = DateTime.Now;
            QuizAmpt.Answers = qA.Answers;
            QuizAmpt.QuizID = id;

            QuizAmpt.Quiz = Quiz;
            QuizAmpt.OfferedCourse = offeredCourse;
            QuizAmpt.Learner = User;

            var arr = QuizAmpt.Answers.Split(new String[] { "!#!#!" }, StringSplitOptions.RemoveEmptyEntries);
            var questions = QuizAmpt.Quiz.Questions.ToList();
            var i = 0;
            QuizAmpt.Marks = 0;
            foreach (var q in questions)
            {
                if (q.Answer == arr[i])
                {
                    QuizAmpt.Marks++;
                }
                i++;
            }
            _db.QuizAttempts.Add(QuizAmpt);
            _db.SaveChanges();
            
            return Json(new ApiResponse
            {
                Message = "Quiz attempted and saved!",
                Status = true,
                Data = new {
                    QuizAmpt.Answers,
                    QuizAmpt.Learner.Username,
                    QuizAmpt.OfferedCourse.OfferedCourseID,
                    oc = QuizAmpt.OfferedCourseID,
                }, 
            }, JsonRequestBehavior.AllowGet);

        }
    }

    class SubimissionTempate
    {
        public String LearnerId { get; set; }
        public String SubmissionTime { get; set; }
    }
}