using Google.Apis.Sheets.v4;
using ImperioNobre.Models;
using ImperioNobre.Models.ViewModels;
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
            var produtos = _sheetsService.LerEstoqueGeral("Produtos");
            return View(produtos);
        }
        public IActionResult Pedidos()
        {
            return View();
        }
        public IActionResult AddPedido()
        {
            var tipoUsuario = HttpContext.Session.GetString("TipoUsuario");

            List<Produto> produtos = new List<Produto>();

            if (tipoUsuario == "Admin")
            {
                produtos = _sheetsService.LerEstoqueVendedor1("Produtos");
            }
            else
            {
                produtos = _sheetsService.LerEstoqueVendedor2("Produtos");
            }

            var clientes = _sheetsService.LerClientes("Clientes");

            var vm = new AddPedidoViewModel
            {
                TipoUsuario = tipoUsuario,
                Produtos = produtos,
                Clientes = clientes
            };

            return View(vm);
        }
    }
}
