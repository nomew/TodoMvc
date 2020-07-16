using System;
using System.Linq;
using System.Web.Mvc;
using TodoMvc.Core;
using TodoMvc.Models;

namespace TodoMvc.Controllers
{
    public class UserController : Controller
    {
        public ActionResult Index()
        {
            return CheckIfLoggedIn();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(User user)
        {
            using (var context = new AppContextMain())
            {
                var data = context.Users.Where(u => u.Username == user.Username && u.Password == user.Password).SingleOrDefault();
                if (data != null)
                {
                    Session["UserId"] = data.UserId;
                    return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Registration()
        {
            return CheckIfLoggedIn();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(User user)
        {
            if (CheckIfUsernameExists(user.Username))
            {
                return Json(new { Result = false, Message = "Username already exists." }, JsonRequestBehavior.AllowGet);
            }
            user.FirstName = user.FirstName.ToLower();
            user.LastName = user.LastName.ToLower();
            user.DateCreated = DateTime.Now;
            user.DateUpdated = DateTime.Now;
            user.Username = user.Username.Trim();
            user.Password = user.Password.Trim();
            user.Status = 1;
            using (var context = new AppContextMain())
            {
                context.Users.Add(user);
                context.SaveChanges();
            }
            return Json(new { Result = true, Message = "User Registration Successful." }, JsonRequestBehavior.AllowGet);
        }

        protected ActionResult CheckIfLoggedIn()
        {
            if (Session["UserId"] != null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View();
            }
        }

        protected bool CheckIfUsernameExists(string username)
        {
            using (var context = new AppContextMain())
            {
                return context.Users.Where(u => u.Username == username).SingleOrDefault() != null;
            }
        }
    }
}