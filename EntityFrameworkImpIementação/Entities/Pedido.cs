using System.Text.Json.Serialization;

namespace EntityFrameworkImpIementação.Entities
{
    public class Pedido
    {
        public Pedido()
        {
            Ativo = true;
        }

        public int PedidoId { get; private set; }
        public string CodigoPedido { get; set; }
        public List<ItemPedido> ItensPedido { get; set; }
        [JsonIgnore]  public bool Ativo { get; set; } 
    }
}
