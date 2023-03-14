using EntityFrameworkImpIementação.Entities;
using System.Text.Json.Serialization;

namespace EntityFrameworkImpIementação.Models
{
    public class ItemPedido
    {
        public int ProdutoId { get; set; }
        public int Quantidade { get; set; }
    }
}
