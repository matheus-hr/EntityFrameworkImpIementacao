namespace EntityFrameworkImpIementação.Models
{
    public class Pedido
    {
        public Pedido()
        {
            ItensPedido = new List<ItemPedido>();
        }

        public int PedidoId { get; private set; }
        public string CodigoPedido { get; set; }
        public List<ItemPedido> ItensPedido { get; set; }
    }
}
