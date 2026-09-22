using Google.Apis.Sheets.v4;
using ImperioNobre.Services;
using Microsoft.AspNetCore.Mvc;

namespace ImperioNobre.Controllers
{
    public class VendedorPainelController : Controller
    {
        private readonly GoogleSheetsService _sheetsService;

        public VendedorPainelController(GoogleSheetsService sheetsService)
        {
            _sheetsService = sheetsService;
        }

        public IActionResult Index()
        {
            var produtos = _sheetsService.LerProdutos("Produtos");
            return View(produtos);
        }
        public IActionResult Pedidos()
        {
            return View();
        }
    }
}
