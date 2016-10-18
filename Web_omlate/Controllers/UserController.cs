using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_omlate.Models;
using Web_omlate.DAL;
using System.Security.Cryptography;
using System.Text;

namespace Web_omlate.Controllers
{
    public class UserController : Controller
    {
        FYPDBContext db;
        public UserController()
        {
            db = new FYPDBContext();
        }

        public ActionResult PageNotFound(string aspxerrorpath)
        {
            return View();
        }
        public ActionResult NotFound(string aspxerrorpath)
        {
            return View();
        }
        public ActionResult Wrong(string aspxerrorpath)
        {
            return View();
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
        public JsonResult Login(string email, string password)
        {
            var pass = CalculateMD5Hash(password);
            var user =
                db.Users.Where(
                    s =>
                        s.Email.Equals(email, StringComparison.CurrentCultureIgnoreCase) && s.Password == pass &&
                        s.Type.ToLower() == "learner").Select(s => new
                        {
                            FirstName = s.FirstName,
                            LastName = s.LastName,
                            Field = s.Field,
                            Email = s.Email,
                            PhoneNo = s.PhoneNo,
                            Type = s.Type
                        }).FirstOrDefault();
            if (user != null)
            {
                return Json(user, JsonRequestBehavior.AllowGet);
            }
            else
            {
                User u = new User();
                u.Email = u.FirstName = u.LastName = u.Password = u.Type = u.Username = u.Field = u.PhoneNo = "null";
                return Json(u, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult LoginIns(string email, string password)
        {
            var pass = CalculateMD5Hash(password);
            var user =
                db.Users.Where(
                    s =>
                        s.Email.Equals(email, StringComparison.CurrentCultureIgnoreCase) && s.Password == pass).Select(s => new
                        {
                            FirstName = s.FirstName,
                            LastName = s.LastName,
                            Field = s.Field,
                            Email = s.Email,
                            PhoneNo = s.PhoneNo,
                            Type = s.Type
                        }).FirstOrDefault();
            if (user != null)
            {
                return Json(user, JsonRequestBehavior.AllowGet);
            }
            else
            {
                User u = new User();
                u.Email = u.FirstName = u.LastName = u.Password = u.Type = u.Username = u.Field = u.PhoneNo = "null";
                return Json(u, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult CheckUsername(string Username)
        {
            var name = db.Users.Where(x => x.Username.Equals(Username, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            if (name == null)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CheckEmail(string Email)
        {
            var email = db.Users.Where(x => x.Email.Equals(Email, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            if (email == null)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        ///User/SaveSignUp        
        public ActionResult SaveSignUp(User model)
        {
            if (ModelState.IsValid)
            {
                var exists = db.Users.Where(u => u.Email.Equals(model.Email, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                if (exists == null)
                {
                    model.Password = CalculateMD5Hash(model.Password);
                    db.Users.Add(model);
                    db.SaveChanges();

                    var type = model.Type;
                    var name = model.Username;
                    if (type == "instructor")
                    {
                        Session["username"] = name;
                        Session["usertype"] = type;
                        Session["email"] = model.Email;
                        Session.Timeout = 30;

                        ViewBag.msg = "Welcome";
                        return RedirectToAction("Index", "Instructor");
                    }
                    else if (type == "learner")
                    {
                        Session["username"] = name;
                        Session["usertype"] = type;
                        Session["email"] = model.Email;

                        Session.Timeout = 30;
                        ViewBag.msg = "Welcome";
                        return RedirectToAction("Index", "Learner");
                    }
                }
                else
                {
                    TempData["warning_msg"] = "User with same email already exists, You can request for new password by just clicking forgot my password!";
                }
            }
            return RedirectToAction("Index", "Default");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult LoginUser(UserLoginModel model)
        {
            if (ModelState.IsValid)
            {
                var pass = CalculateMD5Hash(model.Password);
                var exists = db.Users.Where(u => u.Email.Equals(model.Email, StringComparison.CurrentCultureIgnoreCase) && u.Password == pass).FirstOrDefault();
                if (exists != null)
                {
                    Session["username"] = exists.Username;
                    Session["usertype"] = exists.Type;
                    Session["email"] = exists.Email;
                    Session.Timeout = 30;
                    ViewBag.msg = "Welcome";
                    if (exists.Type == "instructor")
                    {
                        TempData["success_msg"] = "Welcome Back Instructor: " + exists.FirstName + " " + exists.LastName;
                        return RedirectToAction("Index", "Instructor");
                    }
                    else if (exists.Type == "learner")
                    {
                        TempData["success_msg"] = "Welcome Back Learner: " + exists.FirstName + " " + exists.LastName;
                        return RedirectToAction("Index", "Learner");
                    }

                }
                else
                {
                    ViewBag.msg = "Invalid Username or Password";
                    return RedirectToAction("Index", "Default");
                }
            }
            ViewBag.msg = "Invalid Username or Password";
            return RedirectToAction("Index", "Default");
        }

        public ActionResult Index()
        {
            return View();
        }

        public bool VerifyUser(string s)
        {
            if (Session["userId"] != null)
                if (Session["userType"].ToString() == s)
                    return true;
            return false;
        }

        public ActionResult Logout()
        {
            Session.RemoveAll();
            return RedirectToAction("Index", "Default");
        }
    }
}