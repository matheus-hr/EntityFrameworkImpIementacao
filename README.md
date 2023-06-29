# Entity Framework ImpIementação

![project_language](https://img.shields.io/badge/language-C%23-green)
![server_backend](https://img.shields.io/badge/backend%2Fserver-.NET%207-blue)
![web](https://img.shields.io/badge/project-Web%20API-blueviolet)

Aqui será descrito o funcionamento, uso e implementação do entity framework. <br>
Nesse projeto foi implementado o uso do entity framework em memoria, sem a necessidade de um banco de dados. <br>

Foram utilizados os seguintes pacotes: <br>

<b><i>Microsoft.EntityFrameworkCore</i></b><br>
<b><i>Microsoft.EntityFrameworkCore.InMemory</i></b><br>

## Passo 1 - Criação do contexto

1 - Criação da classe de contexto (NomeProjetoContext), por convenção, neste projeto ela foi criada em uma pasta chamada persistence. <br><br>
1.1 - Fazer com que a classe NomeProjetoContext herde de DbContext. <br>

```C#
public class MyDbContext : DbContext
```

1.2 - Criar um construtor que recebe um <i><b>DbContextOptions<MyDbContext> options</b></i> e herde <i><b>base(options)</b></i> <br>

```C#
 public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }
```

1.3 - Criar as Propriedades que serão usados pelo contexto como se fossem as tabelas do banco de dados <br><br>
<b>Obs: Crie as classes com os mesmos campos que devem ser usadas no banco de dados, as classes foram criadas por convenção em uma pasta chamada entities</b>

```C#
 public DbSet<Produto> Produtos {get; set;}
```

1.4 - Implementar a <i><b>sobreescrita</b></i> do metodo <i><b>OnModelCreating</b></i> que é originada da classe <i><b>DbContext</b></i> 

```C#
 protected override void OnModelCreating(ModelBuilder modelBuilder) 
 {
    base.OnModelCreating(modelBuilder);
 
   //Passo 1.5
 }
```

1.5 - Configurar os relacionamento entre as tabelas utilizando o metodo <i><b>modelBuilder.Entity<Classe>()</b></i> br><br>
<b>Obs: Na classe do projeto, estão exemplos de configuração dos relacionamentos de 1 para 1, 1 para Muitos e Muitos para Muitos.</b>

```C#
 modelBuilder.Entity<Pedido>().HasKey(x => x.PedidoId);
                
  // Configura uma relação de Muitos para Um entre ItemPedido e Pedido
  modelBuilder.Entity<ItemPedido>(ip =>
  {
     ip.HasKey(ipe => ipe.ItemPedidoId);
     ip.HasOne(ipe => ipe.Pedido)
       .WithMany(p => p.ItensPedido)
       .HasForeignKey(ipe => ipe.PedidoId);
  });
  
  //Configura uma relação de Muitos para Muitos entre Produto e Fornecedor
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
```

## Passo 2 - Configuração da program.cs

2 - Configuração do Entity framework na classe <b><i>Program.cs</i></b><br> para que possa ser possivel fazer a injeção de dependencia da classe de contexto dentro das controlles

```C#
//Configuração para usar o banco de dados sql server
//Necessario o pacote Microsoft.EntityFrameworkCore.SqlServer

builder.Services.AddDbContext<NomeProjetoContext>(options =>
{
    options.UseSqlServer("SuaStringDeConexaoAqui");
});

//Configuração para usar o banco de dados em memoria
//Necessario o pacote Microsoft.EntityFrameworkCore.InMemory

builder.Services.AddDbContext<NomeProjetoContext>(options =>
{
    options.UseInMemoryDatabase("NomeDoBancoDb");
});
```

## Passo 3 - Utilização do contexto

3 - Implementação do contexto nas controlles e utilização para manipulação dos dados

3.1 - Primeiro é criado uma propriedade privada do contexto e atribuido o seu conteudo via injeção de dependencia.

```C#
public class ProdutoController : ControllerBase
{
    private readonly NomeProjetoContext _contexto;

    public ProdutoController(NomeProjetoContext contexto)
    {
        _contexto = contexto;
    }
}
```

3.2 - Utilizando a propriedade <b><i>_contexto</b></i>, é possivel fazer culsutas chamando a proriedade correspondente a tabela com os metodos de ação do banco.

```C#
//Buscar dados
var listaProdutos = _contexto.Produtos.Where(x => x.Ativo).ToList();
var produto = _contexto.Produtos.Find(id);
    
//Salvar dados
_contexto.Produtos.Add(produto);
_contexto.SaveChanges();
            
//Atualizar dados
_contexto.Produtos.Update(produtoExistente);
_contexto.SaveChanges();
    
//Remover Dados
_contexto.Produtos.Remove(produto);
_contexto.SaveChanges();
```

3.3 - Tambem é possivel trazer tebelas com relacionamentos utilizando o metodo include

```C#
//Relacionamento 1 para Muitos
var Pedidos = _contexto.Pedidos.Include(p => p.ItensPedido)
                               .ThenInclude(i => i.Produto).Where(x => x.Ativo).FirstOrDefault();

//relacionamento Muitos para Muitos
var produtosFornecedor = _contexto.Produtos
                                  .Include(p => p.Fornecedores)
                                  .Where(p => p.Fornecedores.Any(x => x.FornecedorId == fornecedorId && x.Ativo))
                                  .ToList();
```
