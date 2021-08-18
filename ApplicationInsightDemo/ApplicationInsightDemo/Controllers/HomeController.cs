using Microsoft.ApplicationInsights;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ApplicationInsightDemo.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //增加的代码
            var telemetry = new TelemetryClient();
            telemetry.TrackTrace("Execute finished");

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

            throw new Exception("failed");

            return View();
        }
    }
}