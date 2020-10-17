using LanchesMac.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LanchesMac.ViewModels
{
    public class HomeViewModel
    {

        //lógica da view
        public IEnumerable<Lanche>LanchePreferidos { get; set; }

    }
}
