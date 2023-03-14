using EntityFrameworkImpIementação.Entities;
using EntityFrameworkImpIementação.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkImpIementação.Controllers
{
    [Route("/api/fornecedores")]
    public class FornecedorController : ControllerBase
    {
        private readonly MyDbContext _contexto;

        public FornecedorController(MyDbContext contexto)
        {
            _contexto = contexto;
        }

        [HttpGet]
        [Route("get")]
        public IActionResult GetAll()
        {
            var fornecedores = _contexto.Fornecedores.Where(x => x.Ativo).ToList();

            if(fornecedores == null || fornecedores.Count == 0)
                return NotFound();

            return Ok(fornecedores);
        }

        [HttpGet]
        [Route("get/{id}")]
        public IActionResult GetById(int id)
        {
            var fornecedor = _contexto.Fornecedores.Where(x => x.Ativo).FirstOrDefault();
            
            if(fornecedor == null)
                return NotFound();

            return Ok(fornecedor);
        }

        [HttpPost]
        [Route("create")]
        public IActionResult Create([FromBody] Fornecedor fornecedor)
        {
            if(fornecedor == null)
                return BadRequest();

            _contexto.Fornecedores.Add(fornecedor);
            _contexto.SaveChanges();

            return CreatedAtAction(nameof(GetById), new {Id = fornecedor.FornecedorId}, fornecedor);
        }

        [HttpPut]
        [Route("update/{id}")]
        public IActionResult Update(int id, [FromBody] Fornecedor fornecedorAtualizado)
        {
            if(fornecedorAtualizado == null || id == 0)
                return UnprocessableEntity("Informe todo os parametos");

            var fornecedorExistente = _contexto.Fornecedores.Find(id);

            if(fornecedorExistente == null)
                return NotFound();

            fornecedorExistente.Nome = fornecedorAtualizado.Nome;
            fornecedorExistente.Cnpj = fornecedorAtualizado.Cnpj;

            _contexto.Fornecedores.Update(fornecedorExistente);
            _contexto.SaveChanges();

            return Ok(fornecedorExistente);
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public IActionResult Delete(int id)
        {
            if (id == 0)
                return UnprocessableEntity();

            var fornecedor = _contexto.Fornecedores.Find(id);

            if(fornecedor == null) 
                return BadRequest();

            fornecedor.Ativo = false;

            _contexto.Fornecedores.Update(fornecedor);
            _contexto.SaveChanges();

            return NoContent();
        }

        [HttpDelete]
        [Route("deletePermanente/{id}")]
        public IActionResult DeletePermanente(int id)
        {
            var fornecedor = _contexto.Fornecedores.Find(id);

            if(fornecedor == null)
                return BadRequest();

            _contexto.Fornecedores.Remove(fornecedor);
            _contexto.SaveChanges();

            return NoContent();
        }

        [HttpGet]
        [Route("GetFornecedoresByProduto/{produtoId}")]
        public IActionResult GetProdutosByFornecedor(int produtoId)
        {
            var produtoFornecedores = _contexto.Fornecedores
                                             .Include(f => f.Produtos)
                                             .Where(x => x.Produtos.Any(a => a.ProdutoId == produtoId && x.Ativo))
                                             .ToList();
            return Ok(produtoFornecedores);
        }
    }
}
