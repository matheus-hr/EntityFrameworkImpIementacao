using System.Text.Json.Serialization;

namespace EntityFrameworkImpIementação.Entities
{
    public class ItemPedido
    {
        public ItemPedido()
        {
            Ativo = true;
            Produto = new Produto();
        }

        public int ItemPedidoId { get; private set; }
        [JsonIgnore] public Pedido Pedido { get; set; }
        public int PedidoId { get; private set; }
        public Produto Produto { get; set; }
        public int Quantidade { get; set; }
        [JsonIgnore] public bool Ativo { get; set; }
    }
}
