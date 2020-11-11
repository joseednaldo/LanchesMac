using LanchesMac.Models;
using LanchesMac.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        
        public IActionResult Chekout()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Chekout(Pedido pedido)
        {
            var itens = _carrinhoCompra.GetCarrinhoCompraItens();
            _carrinhoCompra.CarrinhoCompraItens = itens;

            if (_carrinhoCompra.CarrinhoCompraItens.Count == 0)
            {
                ModelState.AddModelError("", "Seu carrinho  esta vazio, inclua um lanche...");
            }

            if (ModelState.IsValid)
            {
                _pedidoRepository.CriarPedido(pedido);
                _carrinhoCompra.LimparCarrinho();

                //TempData["Cliente"] = pedido.Nome;
                //TempData["NumeroPedido"] = pedido.PedidoId;

                //TempData["DataPedido"] = pedido.PedidoEnviado;
                ViewBag["TotalPedido"] = _carrinhoCompra.GetCarrinhoCompraTotal();


                _carrinhoCompra.LimparCarrinho();
                return View("~/Views/Pedido/ChekoutCompleto.cshtml",pedido);
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
