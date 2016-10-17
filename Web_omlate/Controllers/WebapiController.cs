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

namespace Web_omlate.Controllers
{
    public class WebapiController : ApiController
    {
        FYPDBContext _db;
        public WebapiController()
        {
            _db = new FYPDBContext();
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

    }
}