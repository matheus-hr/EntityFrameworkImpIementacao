using EntityFrameworkImpIementação.Entities;
using EntityFrameworkImpIementação.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkImpIementação.Controllers
{
    [Route("api/pedidos")]
    public class PedidoController : ControllerBase
    {
        private readonly MyDbContext _contexto;

        public PedidoController(MyDbContext contexto)
        {
            _contexto = contexto;
        }

        [HttpGet]
        [Route("get")]
        public IActionResult GetAll()
        {
            var Pedidos = _contexto.Pedidos
                            .Include(p => p.ItensPedido).ThenInclude(i => i.Produto)
                            .Where(x => x.Ativo).ToList();

            if (Pedidos == null || Pedidos.Count == 0)
                return NotFound();

            return Ok(Pedidos);
        }

        [HttpGet]
        [Route("get/{id}")]
        public IActionResult GetById(int id)
        {
            var Pedidos = _contexto.Pedidos.Include(p => p.ItensPedido)
                                           .ThenInclude(i => i.Produto).Where(x => x.Ativo).FirstOrDefault();

            if (Pedidos == null)
                return NotFound();

            return Ok(Pedidos);
        }

        [HttpPost]
        [Route("create")]
        public IActionResult Create([FromBody] Models.Pedido pedidoDados)
        {
            if(pedidoDados == null)
                return BadRequest();

            var pedidoNovo = new Pedido();
            pedidoNovo.CodigoPedido = pedidoDados.CodigoPedido;


            foreach(var dadosItemPedido in pedidoDados.ItensPedido)
            {
                var produtoItemPedido = _contexto.Produtos.Find(dadosItemPedido.ProdutoId);
                if (produtoItemPedido == null)
                    return BadRequest($"Produto {dadosItemPedido.ProdutoId} não Cadastrado");

                var itemPedidoNovo = new ItemPedido();
                itemPedidoNovo.Produto = produtoItemPedido;
                itemPedidoNovo.Quantidade = dadosItemPedido.Quantidade;

                pedidoNovo.ItensPedido.Add(itemPedidoNovo);
            }   

            _contexto.Pedidos.Add(pedidoNovo);
            _contexto.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = pedidoNovo.PedidoId }, pedidoNovo);
        }
       
        [HttpDelete]
        [Route("delete/{id}")]
        public IActionResult delete(int id)
        {
            var pedido = _contexto.Pedidos.Find(id);

            if(pedido == null) 
                return NotFound();

            pedido.Ativo = false;
            _contexto.Pedidos.Update(pedido);
            _contexto.SaveChanges();

            return NoContent();
        }

        [HttpDelete]
        [Route("deletePermanente/{id}")]
        public IActionResult deletePermanente(int id)
        {
            var pedido = _contexto.Pedidos.Find(id);

            if(pedido == null)
                return NotFound();

            _contexto.Pedidos.Remove(pedido);
            return NoContent();
        }
    }
}
