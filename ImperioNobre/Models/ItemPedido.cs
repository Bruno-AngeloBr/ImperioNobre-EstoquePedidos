using ImperioNobre.Models;

namespace ImperioNobre.Models
{
    public class ItemPedido
    {
        public int PedidoID { get; set; }
        public int NumeroRemessa { get; set; }
        public int ClienteID { get; set; }
        public int ProdutoID { get; set; }
        public int Quantidade { get; set; }
        public double TotalItem { get; set; }
    }
}
