    using Microsoft.AspNetCore.Mvc;

namespace DevOrbitAPI.Controllers.Features.Boards
{
    public class KanbanTaskController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
