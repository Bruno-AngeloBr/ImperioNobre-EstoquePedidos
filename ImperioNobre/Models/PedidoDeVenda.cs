using ImperioNobre.Models.Enums;

namespace ImperioNobre.Models
{
    public class PedidoDeVenda
    {
        public int NumeroPedido { get; set; }
        public Cliente Cliente { get; set; }
        public List<Item> ListaItens { get; set; }
        public DateTime DataEntrega { get; set; }
        public DateTime DataPagamento { get; set; }
        public double TotalPedido { get; set; }
        public int NumeroRemessa { get; set; }
        public StatusPedido StatusPedido { get; set; }

    }
}
