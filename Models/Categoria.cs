using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LanchesMac.Models
{
    public class Categoria
    {
        public int CategoriaId { get; set; }

        [StringLength(100)]
        public string CategoriaNome { get; set; }

        [StringLength(250)]
        public string Descricao { get; set; }
        public List<Lanche> Lanches { get; set; }  // relacionamnetoi 1:N  (1 pra muitos)

    }
}