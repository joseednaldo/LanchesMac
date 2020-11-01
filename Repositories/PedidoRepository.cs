using LanchesMac.Context;
using LanchesMac.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LanchesMac.Repositories
{
    public class PedidoRepository : IPedidoRepository
    {
        private readonly AppDbContext _contexto;
        private readonly CarrinhoCompra _carrinhocompra;
        public PedidoRepository(AppDbContext contexto, CarrinhoCompra carrinhocompra)
        {
            _carrinhocompra = carrinhocompra;
            _contexto = contexto;
        }

      

        public void CriarPedido(Pedido pedido)
        {
            //persistir no banco a compra
            pedido.PedidoEnviado = DateTime.Now;
            _contexto.Pedidos.Add(pedido);

            var carrinhpCompraItens = _carrinhocompra.CarrinhoCompraItens;
            foreach (var carrinhoItem in carrinhpCompraItens)
            {
                var pedidoDetalhe = new PedidoDetalhe()
                {
                    Quantidade = carrinhoItem.Quantidade,
                    LancheId = carrinhoItem.Lanche.LancheId,
                    PedidoId = pedido.PedidoId,
                    Preco = carrinhoItem.Lanche.Preco
                };

                _contexto.PedidoDetalhes.Add(pedidoDetalhe);
            }

            _contexto.SaveChanges();

        }
    }
}
