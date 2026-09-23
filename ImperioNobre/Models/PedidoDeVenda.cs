using ImperioNobre.Models.Enums;

namespace ImperioNobre.Models
{
    public class PedidoDeVenda
    {
        public int PedidoID { get; set; }
        public List<ItemPedido> ListaItens { get; set; }
        public DateTime DataEntrega { get; set; }
        public DateTime DataPagamento { get; set; }
        public double TotalPedido { get; set; }
        public StatusPedido StatusPedido { get; set; }
    }
}
