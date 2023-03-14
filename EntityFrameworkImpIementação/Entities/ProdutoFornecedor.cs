using System.Text.Json.Serialization;

namespace EntityFrameworkImpIementação.Entities
{
    public class ProdutoFornecedor
    {
        public ProdutoFornecedor()
        {
            Ativo = true;
        }

        public int ProdutoFornecedorId { get; private set; }
        public int ProdutoId { get; set; }
        [JsonIgnore] public Produto Produto { get; set; }
        public int FornecedorId { get; set; }
        [JsonIgnore] public Fornecedor Fornecedor { get; set; }
        [JsonIgnore] public bool Ativo { get; set; }
    }
}
