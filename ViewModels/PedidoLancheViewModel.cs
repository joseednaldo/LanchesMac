using LanchesMac.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LanchesMac.ViewModels
{
    public class PedidoLancheViewModel
    {
        public Pedido Pedido { get; set; }
        public IEnumerable<PedidoDetalhe>PedidoDetalhes { get; set; }
    }
}
