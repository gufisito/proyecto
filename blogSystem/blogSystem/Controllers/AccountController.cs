using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.IO;
using blogSystem.Models;
using Microsoft.Web.Helpers;

namespace blogSystem.Controllers
{
    public class AccountController : Controller
    {

        public ActionResult Index(PerfilModel model)
        {
            ViewBag.lista = model.ViewInfo(User.Identity.Name);
            return View();
        }

        //
        // GET: /Account/LogOn

        public ActionResult LogOn()
        {
            return View();
        }

        //
        // POST: /Account/LogOn

        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (Membership.ValidateUser(model.UserName, model.Password))
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Account");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "The user name or password provided is incorrect.");
                
                    if (Session["captcha"] != null && (int)Session["captcha"] > 2 && !ReCaptcha.Validate(privateKey: "6Leh1NESAAAAAAp3pycWl95q8VNxltFX8mnveIjk"))
                    {
                        ModelState.AddModelError("", "Verificar nuevamente imagen captcha");
                    }

                    if (Session["captcha"] == null)
                    {
                        Session["captcha"] = 0;
                    }
                    int c = (int)Session["captcha"];
                    c++;
                    Session["captcha"] = c;
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/LogOff

        public ActionResult LogOff()
        {
            Session.Abandon();
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/Register

        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register

        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                MembershipCreateStatus createStatus;
                Membership.CreateUser(model.UserName, model.Password, model.Email, null, null, true, null, out createStatus);

                if (createStatus == MembershipCreateStatus.Success)
                {
                    linqClassDataContext ctx = new linqClassDataContext();
                    Guid idUs = (from usuario in ctx.aspnet_Users where usuario.UserName == model.UserName select usuario.UserId).ToArray()[0];
                    Guid isRol = (from rol in ctx.aspnet_Roles where rol.RoleName == "Usuario" select rol.RoleId).ToArray()[0];
                    aspnet_UsersInRole rel = new aspnet_UsersInRole() { RoleId=isRol, UserId=idUs };
                    ctx.aspnet_UsersInRoles.InsertOnSubmit(rel);
                    ctx.SubmitChanges();
                    FormsAuthentication.SetAuthCookie(model.UserName, false /* createPersistentCookie */);
                    return RedirectToAction("AddProfileInfo", "Account");
                }
                else
                {
                    ModelState.AddModelError("", ErrorCodeToString(createStatus));
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ChangePassword

        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Account/ChangePassword

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {

                // ChangePassword will throw an exception rather
                // than return false in certain failure scenarios.
                bool changePasswordSucceeded;
                try
                {
                    MembershipUser currentUser = Membership.GetUser(User.Identity.Name, true /* userIsOnline */);
                    changePasswordSucceeded = currentUser.ChangePassword(model.OldPassword, model.NewPassword);
                }
                catch (Exception)
                {
                    changePasswordSucceeded = false;
                }

                if (changePasswordSucceeded)
                {
                    return RedirectToAction("ChangePasswordSuccess");
                }
                else
                {
                    ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ChangePasswordSuccess

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        // GET: /Account/AddProfileInfo
        public ActionResult AddProfileInfo()
        {
            return View();
        }
        
        // POST: /Account/AddProfile
        [HttpPost]
        public ActionResult AddProfileInfo(string Nombre, string Apellido, string Ubicacion, string Sexo, string Intereses, HttpPostedFileBase Avatar, PerfilModel model)
        {
            if (ModelState.IsValid)
            {
                var data = new byte[Avatar.ContentLength];
                Avatar.InputStream.Read(data, 0, Avatar.ContentLength);
                var path = ControllerContext.HttpContext.Server.MapPath("/Content/images/");
                var filename = Path.Combine(path, Path.GetFileName(Avatar.FileName));
                System.IO.File.WriteAllBytes(Path.Combine(path, filename), data);
                model.Nombre = Nombre;
                model.Apellido = Apellido;
                model.Intereses = Intereses;
                model.Ubicacion = Ubicacion;
                model.Sexo = Sexo;
                model.Avatar = Path.GetFileName(Avatar.FileName).ToString();
                model.AddPerfilInfo(model, User.Identity.Name);
                
                FormsAuthentication.SetAuthCookie(User.Identity.Name, false);
                return RedirectToAction("ViewInfo", "Profile");
            }
            return View();
        }

        #region Status Codes
        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }   
}
