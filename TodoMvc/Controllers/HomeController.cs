using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using TodoMvc.Core;
using TodoMvc.Models;

namespace TodoMvc.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            using (var context = new AppContextMain())
            {
                User user = context.Users.Find(Convert.ToInt32(Session["UserId"].ToString()));
                TempData["FullName"] = $"{user.FirstName} {user.LastName}";
            }
            return CheckIfLoggedIn();
        }

        protected ActionResult CheckIfLoggedIn()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Index", "User");
            }
            else
            {
                return View();
            }
        }

        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Index", "User");
        }

        public new ActionResult Profile()
        {
            return CheckIfLoggedIn();
        }

        [HttpGet]
        public ActionResult ReadProfile()
        {
            using (var context = new AppContextMain())
            {
                User user = context.Users.Find(Convert.ToInt32(Session["UserId"].ToString()));
                var data = new
                {
                    Username = user.Username,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Gender = user.Gender,
                    BirthDate = user.BirthDate
                };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateProfile(User user)
        {
            using (var context = new AppContextMain())
            {
                int userId = Convert.ToInt32(Session["UserId"].ToString());
                if (CheckIfUsernameExists(user.Username.Trim(), userId))
                {
                    return Json(new { Success = false, Message = "Username already exists." }, JsonRequestBehavior.AllowGet);
                }
                User temp = context.Users.Find(userId);
                if ((temp.FirstName != user.FirstName.ToLower()) || (temp.LastName != user.LastName.ToLower()) || (temp.Gender != user.Gender) || (temp.BirthDate != user.BirthDate) || (temp.Username != user.Username.Trim()))
                {
                    temp.FirstName = user.FirstName.ToLower().Trim();
                    temp.LastName = user.LastName.ToLower().Trim();
                    temp.Gender = user.Gender;
                    temp.BirthDate = user.BirthDate;
                    temp.Username = user.Username.Trim();
                    temp.DateUpdated = DateTime.Now;
                    context.Entry(temp).State = EntityState.Modified;
                    context.SaveChanges();
                    return Json(new { Success = true, Message = "Profile updated successfully." }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        protected bool CheckIfUsernameExists(string username, int id)
        {
            using (var context = new AppContextMain())
            {
                return context.Users.Where(u => u.Username == username && u.UserId != id).SingleOrDefault() != null;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(string CurrentPassword, string NewPassword, string ConfirmPassword)
        {
            using (var context = new AppContextMain())
            {
                User user = context.Users.Find(Convert.ToInt32(Session["UserId"].ToString()));
                if (user.Password != CurrentPassword.Trim())
                {
                    return Json(new { Success = false, Message = "The current password you have entered is incorrect.", Error = 0 }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    if (user.Password == NewPassword.Trim())
                    {
                        return Json(new { Success = false, Message = "New password must be different from current password.", Error = 1 }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        user.Password = NewPassword.Trim();
                        user.DateUpdated = DateTime.Now;
                        context.Entry(user).State = EntityState.Modified;
                        context.SaveChanges();
                        Session.Clear();
                        Session.Abandon();
                        return Json(new { Success = true, Message = "Password Changed successfully." }, JsonRequestBehavior.AllowGet);
                    }

                }
            }
        }
    }
}