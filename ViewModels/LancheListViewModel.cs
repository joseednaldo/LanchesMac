using LanchesMac.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LanchesMac.ViewModels
{
    public class LancheListViewModel
    {
        // O que eu quero exibir na minha view....
        public IEnumerable<Lanche> Lanches { get; set; }
        public string CategoriaAtual { get; set; }

    }
}
