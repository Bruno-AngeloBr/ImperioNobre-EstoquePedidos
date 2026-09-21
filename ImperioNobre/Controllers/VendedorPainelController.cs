using Microsoft.AspNetCore.Mvc;

namespace ImperioNobre.Controllers
{
    public class VendedorPainelController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
