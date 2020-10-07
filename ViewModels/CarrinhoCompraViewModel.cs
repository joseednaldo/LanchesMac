using LanchesMac.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LanchesMac.ViewModels
{
    public class CarrinhoCompraViewModel
    {
        // é o que eu quero representa na view...
        public CarrinhoCompra  CarrinhoCompra { get; set; }
        public decimal CarrinhoCompraTotal { get; set; }
    }
}
