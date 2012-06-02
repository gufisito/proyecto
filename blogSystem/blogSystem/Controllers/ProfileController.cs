using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using blogSystem.Models;

namespace blogSystem.Controllers
{
    public class ProfileController : Controller
    {
        //
        // GET: /Profile/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ViewInfo(PerfilModel model)
        {
            ViewBag.lista = model.ViewInfo(User.Identity.Name);
            return View();
        }

    }
}
