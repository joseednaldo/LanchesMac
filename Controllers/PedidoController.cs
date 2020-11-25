using LanchesMac.Models;
using LanchesMac.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace LanchesMac.Controllers
{
    public class PedidoController : Controller
    {
        private readonly IPedidoRepository _pedidoRepository;
        private CarrinhoCompra _carrinhoCompra;

        public PedidoController(IPedidoRepository pedidoRepository,
            CarrinhoCompra carrinhoCompra)
        {
            _pedidoRepository = pedidoRepository;
            _carrinhoCompra = carrinhoCompra;
        }

        [Authorize]
        public IActionResult Chekout()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult Chekout(Pedido pedido)
        {
            decimal precoTotalPedido = 0.0m;
            int totalItensPedido = 0;

            List<CarrinhoCompraItem> itens = _carrinhoCompra.GetCarrinhoCompraItens();
            _carrinhoCompra.CarrinhoCompraItens = itens;

            //verifica se existem itens de pedidos
            if (_carrinhoCompra.CarrinhoCompraItens.Count == 0)
            {
                ModelState.AddModelError("", "Seu carrinho  esta vazio, inclua um lanche...");
            }
            //calcula o total do pedido
            foreach (var item in itens)
            {
                totalItensPedido +=item.Quantidade;
                precoTotalPedido += (item.Lanche.Preco * item.Quantidade);
            }

            //atribuindo o total de itens do pedido
            pedido.TotalItensPedido = totalItensPedido;

            //atribui o total do pedidod ao pedido
            pedido.PedidoTotal = precoTotalPedido;


            if (ModelState.IsValid)
            {
                _pedidoRepository.CriarPedido(pedido);

                ViewBag.TotalPedido = _carrinhoCompra.GetCarrinhoCompraTotal();
                ViewBag.CheckoutCompletoMensagem = "Obrigado pelo seu pedido :) ";

                _carrinhoCompra.LimparCarrinho();
                return View("~/Views/Pedido/CheckoutCompleto.cshtml", pedido);
            }

            return View(pedido);
        }

        public IActionResult ChekoutCompleto()
        {

            ViewBag.Cliente = TempData["Cliente"];
            ViewBag.NumeroPedido = TempData["NumeroPedido"];
            ViewBag.DataPedido = TempData["DataPedido"];
            ViewBag.TotalPedido = TempData["TotalPedido"];

            ViewBag.ChekoutCompletoMensagem = "Obrigado pelo seu pedido :";
            return View();
        }
    }
}
