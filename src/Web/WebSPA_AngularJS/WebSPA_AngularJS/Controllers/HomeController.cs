using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

namespace WebSPA_AngularJS.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        { 
            var mvcName = typeof(Controller).Assembly.GetName();
            var isMono = Type.GetType("Mono.Runtime") != null;

            ViewData["Version"] = mvcName.Version.Major + "." + mvcName.Version.Minor;
            ViewData["Runtime"] = isMono ? "Mono" : ".NET";

            return View();
        }

        [OutputCache(NoStore = true)]
        public ActionResult Configuration()
        {
            return Json(new { 
                PagesUrl = ConfigurationManager.AppSettings["PagesUrl"],
                IdentityUrl = ConfigurationManager.AppSettings["IdentityUrl"]
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
