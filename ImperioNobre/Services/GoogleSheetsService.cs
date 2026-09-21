using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using ImperioNobre.Models;

namespace ImperioNobre.Services
{
    public class GoogleSheetsService
    {
        private readonly SheetsService _service;
        private readonly string _spreadsheetId;
            
        public GoogleSheetsService(IConfiguration config)
        {
            _spreadsheetId = config["GoogleSheets:SpreadsheetId"];
            var credencialPath = config["GoogleSheets:CredentialPath"];

            // 1. Lê o arquivo JSON da credencial
            GoogleCredential credential;
            using (var stream = new FileStream(credencialPath, FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream)
                    .CreateScoped(SheetsService.Scope.Spreadsheets);
            }

            // 2. Cria o serviço do Google Sheets já autenticado
            _service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "ImperioNobre",
            });
        }

        public List<Produto> LerProdutos(string aba)
        {
            // Define o intervalo de colunas que você quer ler
            var range = $"{aba}!A:F";
            var request = _service.Spreadsheets.Values.Get(_spreadsheetId, range);
            var response = request.Execute();
            var linha = response.Values;

            var produtos = new List<Produto>();

            for (int i = 1; i < linha.Count; i++)
            {
                var coluna = linha[i];
                produtos.Add(new Produto
                {
                    ProdutosID = int.Parse(coluna[0].ToString()),
                    Nome = coluna[1].ToString(),
                    Tamanho = int.Parse(coluna[2].ToString()),
                    Sabor = coluna[3].ToString(),
                    PrecoUnit = double.Parse(coluna[4].ToString()),
                    QtdDisponivel = int.Parse(coluna[5].ToString()),
                });
            }

            return produtos;
        }

        public void AtualizarQuantidade(string aba, int produtoId, int novaQtd)
        {
            var range = $"{aba}!A";
            var request = _service.Spreadsheets.Values.Get(_spreadsheetId, range);
            var response = request.Execute();
            var linhas = response.Values;

            for (int i = 1; i <= linhas.Count; i++)
            {
                if (int.TryParse(linhas[i][0].ToString(), out int idPlanilha) && idPlanilha == produtoId)
                {
                    var updateRange = $"{aba}!F{i + 1}"; // coluna F na linha informada

                    var valueRange = new ValueRange
                    {
                        Values = new List<IList<object>> { new List<object> { novaQtd } }
                    };

                    var updateRequest = _service.Spreadsheets.Values.Update(valueRange, _spreadsheetId, updateRange);
                    updateRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.RAW;
                    updateRequest.Execute();

                    break;
                }
            }
        }
    }
}
