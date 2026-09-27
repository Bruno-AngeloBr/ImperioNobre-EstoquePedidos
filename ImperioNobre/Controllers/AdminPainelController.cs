using Microsoft.AspNetCore.Mvc;
using ImperioNobre.Models;     // classe Produto
using ImperioNobre.Services;   // classe GoogleSheetsService
using System.IO;
using ImperioNobre.Models.ViewModels;

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
            var vm = new EstoqueViewModel
            {
                EstoqueGeral = _sheetsService.LerEstoqueGeral("Produtos"),
                EstoqueVendedor1 = _sheetsService.LerEstoqueVendedor1("Produtos"),
                EstoqueVendedor2 = _sheetsService.LerEstoqueVendedor2("Produtos")
            };

            return View(vm);
        }

        public IActionResult AddProdutos(int estoque)
        {
            List<Produto> produtos = new List<Produto>();

            //Seleciona qual estoque será lido para alterar
            if (estoque == 1)
            {
                produtos = _sheetsService.LerEstoqueVendedor1("Produtos");
            }
            else if (estoque == 2)
            {
                produtos = _sheetsService.LerEstoqueVendedor2("Produtos");
            }

            var vm = new SelecionarEstoqueViewModel
            {
                Produtos = produtos,
                NumeroEstoque = estoque
            };

            return View(vm); // envia lista para a View AddProdutos.cshtml com os produtos e o estoque selecionado
        }

        [HttpPost]
        public IActionResult SalvarAlteracoes([FromBody] SalvarAlteracoesViewModel dados)
        {
            try
            {
                foreach (var produto in dados.Produtos)
                {
                    // Atualiza cada produto na planilha
                    _sheetsService.AtualizarQuantidade("Produtos", produto.Id, produto.Quantidade, dados.NumeroEstoque);
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
