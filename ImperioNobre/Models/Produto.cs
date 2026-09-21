namespace ImperioNobre.Models
{
    public class Produto
    {
        public int ProdutosID { get; set; }
        public string Nome { get; set; }
        public int Tamanho { get; set; }
        public string Sabor { get; set; }
        public double PrecoUnit { get; set; }
        public int QtdDisponivel { get; set; }

        public Produto()
        {
            
        }
    }
}
