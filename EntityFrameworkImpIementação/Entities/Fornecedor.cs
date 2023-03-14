using System.Text.Json.Serialization;

namespace EntityFrameworkImpIementação.Entities
{
    public class Fornecedor
    {
        public Fornecedor()
        {
            Ativo = true;
        }

        public int FornecedorId { get; private set; }
        public string Nome { get; set; }
        public string Cnpj { get; set; }
        [JsonIgnore] public List<ProdutoFornecedor> Produtos { get; set; }
        [JsonIgnore] public bool Ativo { get; set; }
    }
}
