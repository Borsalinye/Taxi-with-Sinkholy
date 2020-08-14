using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Taxi.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Список заказов";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Наши контакты";

            return View();
        }
    }
}