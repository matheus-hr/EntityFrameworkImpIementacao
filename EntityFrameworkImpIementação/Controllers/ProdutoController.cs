using EntityFrameworkImpIementação.Entities;
using EntityFrameworkImpIementação.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkImpIementação.Controllers
{
    [Route("api/produtos")]
    public class ProdutoController : ControllerBase
    {
        private readonly MyDbContext _contexto;

        public ProdutoController(MyDbContext contexto)
        {
            _contexto = contexto;
        }

        [HttpGet]
        [Route("get")]
        public IActionResult GetAll()
        {
            var produtos = _contexto.Produtos.Where(x => x.Ativo).ToList();

            if(produtos == null || produtos.Count == 0)
                return NotFound();

            return Ok(produtos);
        }

        [HttpGet]
        [Route("get/{id}")]
        public IActionResult GetById(int id)
        {
            var produto = _contexto.Produtos.Find(id);

            if(produto == null)
                return NotFound();

            return Ok(produto);
        }

        [HttpPost]
        [Route("create")]
        public IActionResult Create([FromBody] Produto produto)
        {
            if (produto == null)
                return BadRequest();

            _contexto.Produtos.Add(produto);
            _contexto.SaveChanges();

            return CreatedAtAction(nameof(GetById), new {id = produto.ProdutoId}, produto);
        }

        [HttpPut]
        [Route("update/{id}")]
        public IActionResult Update(int id, [FromBody] Produto produtoAtualizado)
        {
            if (produtoAtualizado == null || id == 0)
                return UnprocessableEntity("Informe todo os parametos");

            var produtoExistente = _contexto.Produtos.Find(id);

            if (produtoExistente == null )
                return NotFound();

            produtoExistente.Tipo = produtoAtualizado.Tipo;
            produtoExistente.Nome = produtoAtualizado.Nome;
            produtoExistente.Marca = produtoAtualizado.Marca;
            produtoExistente.Preco = produtoAtualizado.Preco;

            _contexto.Produtos.Update(produtoExistente);
            _contexto.SaveChanges();

            return Ok(produtoExistente);
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public IActionResult Delete(int id)
        {
            var produto = _contexto.Produtos.Where(x => x.ProdutoId == id).FirstOrDefault();
            if(produto == null)
                return NotFound();

            produto.Ativo = false;

            _contexto.Produtos.Update(produto);
            _contexto.SaveChanges();

            return NoContent();
        }

        [HttpDelete]
        [Route("DeletePermanete/{id}")]
        public IActionResult DeletePermanete(int id)
        {
            var produto = _contexto.Produtos.Find(id);

            if(produto == null)
                return NotFound();

            _contexto.Produtos.Remove(produto);
            _contexto.SaveChanges();

            return NoContent();
        }

        [HttpGet]
        [Route("GetProdutosByFornecedor/{fornecedorId}")]
        public IActionResult GetProdutosByFornecedor(int fornecedorId)
        {
            var produtosFornecedor = _contexto.Produtos
                                              .Include(p => p.Fornecedores)
                                              .Where(p => p.Fornecedores.Any(x => x.FornecedorId == fornecedorId && x.Ativo))
                                              .ToList();
            return Ok(produtosFornecedor);
        }

        [HttpPost]
        [Route("CreateProdutoFornecedor")]
        public IActionResult CreateProdutoFornecedor([FromBody] ProdutoFornecedor produtoFornecedor)
        {
            if (produtoFornecedor == null)
                return BadRequest();

            var produto = _contexto.Produtos.Where(x => x.ProdutoId == produtoFornecedor.ProdutoId && x.Ativo).FirstOrDefault();
            if (produto == null)    
                return BadRequest();

            var fornecedor = _contexto.Fornecedores.Where(x => x.FornecedorId == produtoFornecedor.FornecedorId && x.Ativo).FirstOrDefault();
            if(fornecedor == null)
                return BadRequest();

            _contexto.ProdutosFornecedores.Add(produtoFornecedor);
            _contexto.SaveChanges(true);

            return Ok();
        }
    }
}
