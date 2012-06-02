using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using blogSystem.Models;

namespace blogSystem.Controllers
{
    public class TutorialController : Controller
    {
        //
        // GET: /Tutorial/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(PublicModel model)
        {
            ViewBag.lista = model;
            return View(model);
        }

    }
}
