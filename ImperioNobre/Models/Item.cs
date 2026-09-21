using ImperioNobre.Models;

namespace ImperioNobre.Models
{
    public class Item
    {
        public int LinhaID { get; set; }
        public Produto Produto { get; set; }
        public int Quantidade { get; set; }
        public int NumeroPedido { get; set; }
        public double TotalLinha { get; set; }

    }
}
