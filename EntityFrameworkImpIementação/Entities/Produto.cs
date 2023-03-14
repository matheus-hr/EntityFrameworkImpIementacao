using System.Text.Json.Serialization;

namespace EntityFrameworkImpIementação.Entities
{
    public class Produto
    {
        public Produto()
        {
            Ativo = true;
        }

        public int ProdutoId { get; private set; }
        public string? Nome { get; set; }
        public string? Tipo { get; set; }
        public string? Marca { get; set; }
        public decimal Preco { get; set; }
        [JsonIgnore] public List<ProdutoFornecedor> Fornecedores { get; set; }
        [JsonIgnore] public bool Ativo { get; set; }
    }
}
