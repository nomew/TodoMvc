using System.Web.Mvc;

namespace TodoMvc.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        [ActionName("404")]
        public ActionResult Error404()
        {
            return View("Error");
        }
    }
}