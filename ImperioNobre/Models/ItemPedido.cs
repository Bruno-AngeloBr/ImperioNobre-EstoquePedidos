using ImperioNobre.Models;

namespace ImperioNobre.Models
{
    public class ItemPedido
    {
        public int ProdutoId { get; set; }
        public int Quantidade { get; set; }
        public double Preco { get; set; }
        public double Desconto { get; set; }
        public double TotalItem { get; set; }
    }
}
