namespace ImperioNobre.Models.ViewModels
{
    public class AddPedidoViewModel
    {
        public string TipoUsuario { get; set; }
        public List<Produto> Produtos { get; set; }
        public List<Cliente> Clientes { get; set; }
    }
}
