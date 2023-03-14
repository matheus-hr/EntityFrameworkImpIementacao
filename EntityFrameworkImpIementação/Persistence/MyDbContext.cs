using EntityFrameworkImpIementação.Entities;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkImpIementação.Persistence
{
    public class MyDbContext : DbContext //1 - faz a classe Herdar a classe DbContext 
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) {

        } //2 - Cria um construtor que recebe como parametro uma Opção do DbContext do tipo ProjectDbContext

        public DbSet<Produto> Produtos {get; set;} //3 - Cria uma propriedade do tipo DbSet<ModelQueRepresentaTabela>, essa prorpiedade será a entidade que representa a tabela no banco de dados.
        public DbSet<Pedido> Pedidos { get; set;}
        public DbSet<ItemPedido> ItensPedido { get; set;}
        public DbSet<Fornecedor> Fornecedores { get; set; }
        public DbSet<ProdutoFornecedor> ProdutosFornecedores { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) //4 - Esse método é usado para configurar o modelo de dados que será usado pelo contexto do banco de dados.
        {
            base.OnModelCreating(modelBuilder);

            // 4.1 - Configura as chaves primarias das tabelas 
            modelBuilder.Entity<Pedido>()
                .HasKey(x => x.PedidoId);

            modelBuilder.Entity<Fornecedor>()
                .HasKey(x => x.FornecedorId);

            modelBuilder.Entity<Produto>()
                .HasKey(x => x.ProdutoId);

            // 4.3 - Configura uma relação de Muitos para Muitos entre Produto e Fornecedor
            modelBuilder.Entity<ProdutoFornecedor>(pf =>
            {
                pf.HasKey(pfe => pfe.ProdutoFornecedorId);

                pf.HasOne(pe => pe.Produto)
                  .WithMany(pfe => pfe.Fornecedores)
                  .HasForeignKey(pe => pe.ProdutoId);

                pf.HasOne(fe => fe.Fornecedor)
                  .WithMany(pfe => pfe.Produtos)
                  .HasForeignKey(fe => fe.FornecedorId);
            });

            // 4.3 - Configura uma relação de Muitos para Um entre ItemPedido e Pedido
            modelBuilder.Entity<ItemPedido>(ip =>
            {
                ip.HasKey(ipe => ipe.ItemPedidoId);
                ip.HasOne(ipe => ipe.Produto);
                ip.HasOne(ipe => ipe.Pedido).WithMany(p => p.ItensPedido).HasForeignKey(ipe => ipe.PedidoId);
            });
        }
    }
}
