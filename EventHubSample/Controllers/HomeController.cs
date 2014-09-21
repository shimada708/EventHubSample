using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EventHubSample.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            Trace.WriteLine(String.Format("Request on /Home/Index by {0}", Request.UserAgent));
            return View();
        }

        public ActionResult About()
        {
            Trace.WriteLine(String.Format("Request on /Home/About by {0}", Request.UserAgent));
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            Trace.WriteLine(String.Format("Request on /Home/Contact by {0}", Request.UserAgent));
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}