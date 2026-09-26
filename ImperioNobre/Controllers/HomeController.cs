using ImperioNobre.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using ImperioNobre.Models.ViewModels;

namespace ImperioNobre.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        //Id da Planiha
        private readonly string _spreadsheetId;

        private readonly SheetsService service;

        private readonly string range = "Usuarios!A:C";

        public HomeController(ILogger<HomeController> logger, IConfiguration config)
        {
            _logger = logger;

            // Obtém o ID da planilha
            _spreadsheetId = config["GoogleSheets:SpreadsheetId"];

            if (string.IsNullOrWhiteSpace(_spreadsheetId))
            {
                throw new InvalidOperationException(
                    "A configuração 'GoogleSheets:SpreadsheetId' não foi encontrada."
                );
            }

            GoogleCredential credential;

            // =========================================================
            // LOCAL
            // =========================================================
            // Se existir um caminho para o arquivo JSON, usa o arquivo.
            var credencialPath = config["GoogleSheets:CredentialPath"];

            if (!string.IsNullOrWhiteSpace(credencialPath))
            {
                credential = GoogleCredential.FromFile(credencialPath).CreateScoped(SheetsService.Scope.Spreadsheets);
            }
            else
            {
                // =====================================================
                // AZURE
                // =====================================================
                // No Azure, a credencial será armazenada como JSON
                // nas configurações do App Service.
                var credentialsJson = config["GoogleSheets:CredentialsJson"];

                if (string.IsNullOrWhiteSpace(credentialsJson))
                {
                    throw new InvalidOperationException(
                        "Nenhuma credencial do Google foi configurada. " +
                        "Configure 'GoogleSheets:CredentialPath' para execução local " +
                        "ou 'GoogleSheets:CredentialsJson' no Azure."
                    );
                }

                credential = GoogleCredential.FromJson(credentialsJson).CreateScoped(SheetsService.Scope.Spreadsheets);
            }

            // Cria o serviço do Google Sheets
            service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "ImperioNobre"
            });
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string Nome,string Senha)
        {
            // 1. Cria a requisição para buscar os dados da aba Usuarios
            var request = service.Spreadsheets.Values.Get(_spreadsheetId, "Usuarios!A:C");

            // 2. Executa a requisição e recebe a resposta do Google Sheets
            var response = request.Execute();

            // 3. Extrai os valores da resposta (linhas e colunas da planilha)
            var values = response.Values;

            // 4. Verifica se a planilha tem dados
            if (values != null && values.Count > 0)
            {
                // 5. Percorre cada linha da planilha
                foreach ( var row in values )
                {
                    // Proteção: garante que a linha tem pelo menos 2 colunas (Nome e Senha)
                    if (row.Count < 2) continue;

                    // 6. Pega os valores das colunas
                    var nomePlanilha = row[0].ToString(); // Coluna A
                    var senhaPlanilha = row[1].ToString(); // Coluna B
                    var tipoUsuario = row.Count > 2 ? row[2].ToString() : ""; // Coluna C (informação extra)

                    // 7. Compara com o que o usuário digitou
                    if (Nome == nomePlanilha && Senha == senhaPlanilha)
                    {
                        // 8. Se bater, login válido
                        TempData["TipoUsuario"] = tipoUsuario; // guarda o tipo para usar depois

                        if (tipoUsuario == "Admin")
                            return RedirectToAction("Index", "AdminPainel");
                        else
                            return RedirectToAction("Index", "VendedorPainel"); 
                    }
                }
            }
            // 9. Se não encontrou nenhum usuário válido
            ViewBag.MensagemErro = "Usuário ou senha inválidos.";
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
