using Google.Apis.Sheets.v4;
using ImperioNobre.Models;
using ImperioNobre.Models.ViewModels;
using ImperioNobre.Services;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

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
        
        [HttpPost]
        public IActionResult SalvarItensPedido([FromBody] SalvarPedidoViewModel dados)
        {
            try
            {
                if (dados == null)
                    return BadRequest("Os dados da remessa não foram informados.");

                if (dados.ItensPedido == null || !dados.ItensPedido.Any())
                    return BadRequest("A remessa precisa conter pelo menos um item.");

                string nomeAba = $"RM{dados.NumeroRemessa}";

                if (!_sheetsService.ExisteAba(nomeAba))
                {
                    return BadRequest($"A aba '{nomeAba}' não existe. O salvamento não foi realizado.");
                }

                _sheetsService.AdicionarInformacoesRemessa(nomeAba, dados.TipoUsuario, dados.ClientId, dados.DataEntrega, dados.DataPagamento);

                foreach (var produto in dados.ItensPedido)
                {
                    // Atualiza cada produto na planilha
                    _sheetsService.AdicionarItensRemessa(nomeAba, produto.ProdutoId, produto.Quantidade, produto.Preco, produto.Desconto, produto.TotalItem);
                }

                return Ok("Remessa salva com sucesso."); // retorna sucesso (status 200)
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
