namespace ImperioNobre.Models.ViewModels
{
    public class EstoqueViewModel
    {
        public List<Produto> EstoqueGeral { get; set; }
        public List<Produto> EstoqueVendedor1 { get; set; }
        public List<Produto> EstoqueVendedor2 { get; set; }

        public EstoqueViewModel()
        {

        }
    }
}
