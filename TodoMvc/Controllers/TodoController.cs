using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using TodoMvc.Core;
using TodoMvc.Models;

namespace TodoMvc.Controllers
{
    public class TodoController : Controller
    {
        // GET: Todo
        [HttpGet]
        public ActionResult Index()
        {
            List<Todo> todos;
            using (var context = new AppContextMain())
            {
                int UserId = Convert.ToInt32(Session["UserId"].ToString());
                todos = context.Todos.Where(t => t.UserReference == UserId).OrderByDescending(t => t.DateCreated).ToList();
            }
            return Json(todos, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Todo todo)
        {
            if (CheckIfTitleExistsForCreate(todo.Title))
            {
                return Json(new { Result = false, Message = "Title already exists." }, JsonRequestBehavior.AllowGet);
            }

            using (var context = new AppContextMain())
            {
                todo.Title = todo.Title.ToLower();
                todo.Description = todo.Description.ToLower();
                todo.UserReference = Convert.ToInt32(Session["UserId"]);
                todo.IsDone = 0;
                todo.Status = 1;
                todo.DateCreated = DateTime.Now;
                todo.DateUpdated = DateTime.Now;
                context.Todos.Add(todo);
                context.SaveChanges();
                return Json(new { Result = true }, JsonRequestBehavior.AllowGet);
            }
        }

        public bool CheckIfTitleExistsForCreate(string title)
        {
            using (var context = new AppContextMain())
            {
                int UserId = Convert.ToInt32(Session["UserId"].ToString());
                return context.Todos.Where(t => t.Title == title.Trim() && t.UserReference == UserId).SingleOrDefault() != null;
            }
        }

        [HttpPost]
        public ActionResult ChangeIsDoneStatus(int? id)
        {
            using (var context = new AppContextMain())
            {
                Todo todo = context.Todos.Find(id);
                todo.IsDone = todo.IsDone == 1 ? 0 : 1;
                context.Entry(todo).State = EntityState.Modified;
                context.SaveChanges();
            }
            return Json(new { Result = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Todo todo)
        {
            if (CheckIfTitleExistsForEdit(todo.Title, todo.TodoId))
            {
                return Json(new { Result = false, Message = "Title already exists." }, JsonRequestBehavior.AllowGet);
            }

            using (var context = new AppContextMain())
            {
                Todo temp = context.Todos.Find(todo.TodoId);

                if (temp.Title != todo.Title.ToLower() || temp.Description != todo.Description)
                {
                    temp.Title = todo.Title.ToLower();
                    temp.Description = todo.Description;
                    temp.DateUpdated = DateTime.Now;
                    context.Entry(temp).State = EntityState.Modified;
                    context.SaveChanges();
                    return Json(new { Result = true }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { Result = false }, JsonRequestBehavior.AllowGet);
            }
        }

        public bool CheckIfTitleExistsForEdit(string title, int todoId)
        {
            using (var context = new AppContextMain())
            {
                int UserId = Convert.ToInt32(Session["UserId"].ToString());
                return context.Todos.Where(t => t.Title == title && t.UserReference == UserId && t.TodoId != todoId).SingleOrDefault() != null;
            }
        }

        [HttpPost]
        public ActionResult Delete(int? id)
        {

            using (var context = new AppContextMain())
            {
                Todo todo = context.Todos.Find(id);
                context.Todos.Remove(todo);
                context.SaveChanges();
            }
            return Json(new { Result = true }, JsonRequestBehavior.AllowGet);
        }
    }
}