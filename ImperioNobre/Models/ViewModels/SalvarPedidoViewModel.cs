using System.Text.Json.Serialization;

namespace ImperioNobre.Models.ViewModels
{
    public class SalvarPedidoViewModel
    {
        public string TipoUsuario { get; set; }

        [JsonPropertyName("clienteId")]
        public int ClientId { get; set; }
        public int NumeroRemessa { get; set; }
        public DateTime DataEntrega { get; set; }
        public DateTime DataPagamento { get; set; }
        public List<ItemPedido> ItensPedido { get; set; }
    }
}
