using Microsoft.AspNetCore.Mvc;
using ImperioNobre.Models;     // classe Produto
using ImperioNobre.Services;   // classe GoogleSheetsService
using System.IO;

namespace ImperioNobre.Controllers
{
    public class AdminPainelController : Controller
    {
        private readonly GoogleSheetsService _sheetsService;

        public AdminPainelController(GoogleSheetsService sheetsService)
        {
            _sheetsService = sheetsService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Estoque()
        {
            var produtos = _sheetsService.LerProdutos("Produtos");
            return View(produtos);
        }

        public IActionResult AddProdutos()
        {
            var produtos = _sheetsService.LerProdutos("Produtos");
            return View(produtos); // envia lista para a View AddProdutos.cshtml
        }

        [HttpPost]
        public IActionResult SalvarAlteracoes([FromBody] List<ProdutoAlteracao> produtos)
        {
            try
            {
                foreach (var produto in produtos)
                {
                    // Atualiza cada produto na planilha
                    _sheetsService.AtualizarQuantidade("Produtos", produto.Id, produto.Quantidade);
                }

                return Ok(); // retorna sucesso (status 200)
            }
            catch (Exception ex)
            {
                // logar erro se quiser
                Console.WriteLine("Erro ao salvar alterações: " + ex.Message);
                Console.WriteLine("Detalhes: " + ex.StackTrace);
                return StatusCode(500, "Erro ao salvar alterações");
            }
        }


    }
}
