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
                // No Azure, usa o JSON armazenado nas Variáveis de Ambiente.
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

            // Cria o serviço do Google Sheets autenticado
            _service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "ImperioNobre",
            });
        }

        public List<Produto> LerEstoqueGeral(string aba)
        {
            // Define o intervalo de colunas que você quer ler
            var range = $"{aba}!A:F";

            var request = _service.Spreadsheets.Values.Get(
                _spreadsheetId,
                range
            );

            var response = request.Execute();
            var linhas = response.Values;

            var produtosGeral = new List<Produto>();

            if (linhas == null || linhas.Count <= 1)
            {
                return produtosGeral;
            }

            // Começa em 2 para ignorar o cabeçalho
            for (int i = 2; i < linhas.Count; i++)
            {
                var coluna = linhas[i];

                // Garante que existem as 6 colunas necessárias
                if (coluna.Count < 6)
                    continue;

                produtosGeral.Add(new Produto
                {
                    ProdutosID = int.Parse(coluna[0].ToString()),
                    Nome = coluna[1].ToString(),
                    Tamanho = int.Parse(coluna[2].ToString()),
                    Sabor = coluna[3].ToString(),
                    PrecoUnit = double.Parse(coluna[4].ToString()),
                    QtdDisponivel = int.Parse(coluna[5].ToString()),
                });
            }

            return produtosGeral;
        }

        public List<Produto> LerEstoqueVendedor1(string aba)
        {
            // Define o intervalo de colunas que você quer ler
            var range = $"{aba}!H:M";

            var request = _service.Spreadsheets.Values.Get(
                _spreadsheetId,
                range
            );

            var response = request.Execute();
            var linhas = response.Values;

            var produtosGeral = new List<Produto>();

            if (linhas == null || linhas.Count <= 1)
            {
                return produtosGeral;
            }

            // Começa em 2 para ignorar o cabeçalho
            for (int i = 2; i < linhas.Count; i++)
            {
                var coluna = linhas[i];

                // Garante que existem as 6 colunas necessárias
                if (coluna.Count < 6)
                    continue;

                produtosGeral.Add(new Produto
                {
                    ProdutosID = int.Parse(coluna[0].ToString()),
                    Nome = coluna[1].ToString(),
                    Tamanho = int.Parse(coluna[2].ToString()),
                    Sabor = coluna[3].ToString(),
                    PrecoUnit = double.Parse(coluna[4].ToString()),
                    QtdDisponivel = int.Parse(coluna[5].ToString()),
                });
            }

            return produtosGeral;
        }

        public List<Produto> LerEstoqueVendedor2(string aba)
        {
            // Define o intervalo de colunas que você quer ler
            var range = $"{aba}!O:T";

            var request = _service.Spreadsheets.Values.Get(
                _spreadsheetId,
                range
            );

            var response = request.Execute();
            var linhas = response.Values;

            var produtosGeral = new List<Produto>();

            if (linhas == null || linhas.Count <= 1)
            {
                return produtosGeral;
            }

            // Começa em 2 para ignorar o cabeçalho
            for (int i = 2; i < linhas.Count; i++)
            {
                var coluna = linhas[i];

                // Garante que existem as 6 colunas necessárias
                if (coluna.Count < 6)
                    continue;

                produtosGeral.Add(new Produto
                {
                    ProdutosID = int.Parse(coluna[0].ToString()),
                    Nome = coluna[1].ToString(),
                    Tamanho = int.Parse(coluna[2].ToString()),
                    Sabor = coluna[3].ToString(),
                    PrecoUnit = double.Parse(coluna[4].ToString()),
                    QtdDisponivel = int.Parse(coluna[5].ToString()),
                });
            }

            return produtosGeral;
        }

        // Ler tabela clientes
        public List<Cliente> LerClientes(string aba)
        {
            // Define o intervalo de colunas que você quer ler
            var range = $"{aba}!A:B";

            var request = _service.Spreadsheets.Values.Get(
                _spreadsheetId,
                range
            );

            var response = request.Execute();
            var linhas = response.Values;

            var clientesGeral = new List<Cliente>();

            if (linhas == null || linhas.Count <= 1)
            {
                return clientesGeral;
            }

            // Começa em 1 para ignorar o cabeçalho
            for (int i = 1; i < linhas.Count; i++)
            {
                var coluna = linhas[i];

                // Garante que existem as 6 colunas necessárias
                if (coluna.Count < 2)
                    continue;

                clientesGeral.Add(new Cliente
                {
                    ClientID = int.Parse(coluna[0].ToString()),
                    Nome = coluna[1].ToString(),
                });
            }

            return clientesGeral;
        }

        public void AtualizarQuantidade(string aba, int produtoId, int novaQtd, int numeroEstoque)
        {
            string range;

            if (numeroEstoque == 1)
            {
                range = $"{aba}!H:H";
            }
            else
            {
                range = $"{aba}!O:O";
            }

            var request = _service.Spreadsheets.Values.Get(
                _spreadsheetId,
                range
            );

            var response = request.Execute();
            var linhas = response.Values;

            if (linhas == null)
                return;

            // Começa em 2 para ignorar o cabeçalho
            for (int i = 2; i < linhas.Count; i++)
            {
                if (linhas[i].Count == 0)
                    continue;

                if (int.TryParse(
                    linhas[i][0]?.ToString(),
                    out int idPlanilha)
                    && idPlanilha == produtoId)
                {
                    // A planilha começa na linha 2.
                    // Como i começa em 2 após o cabeçalho,
                    // a linha real é i + 1.

                    string updateRange;

                    if (numeroEstoque == 1)
                    {
                        updateRange = $"{aba}!M{i + 1}";
                    }
                    else
                    {
                        updateRange = $"{aba}!T{i + 1}";
                    }

                    var valueRange = new ValueRange
                    {
                        Values = new List<IList<object>>
                        {
                            new List<object> { novaQtd }
                        }
                    };

                    var updateRequest =
                        _service.Spreadsheets.Values.Update(
                            valueRange,
                            _spreadsheetId,
                            updateRange
                        );

                    updateRequest.ValueInputOption =
                        SpreadsheetsResource.ValuesResource
                            .UpdateRequest.ValueInputOptionEnum.RAW;

                    updateRequest.Execute();

                    break;
                }
            }
        }
    }
}