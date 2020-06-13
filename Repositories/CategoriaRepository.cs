using LanchesMac.Context;
using LanchesMac.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LanchesMac.Repositories
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly AppDbContext _contexto;
        public CategoriaRepository(AppDbContext contexto)
        {
            //injeção de dependencia do contexto...pra ter acesso ao EF...
            _contexto = contexto;
        }

        IEnumerable<Categoria> ICategoriaRepository.Categorias => _contexto.Categorias.ToList();

        //public IEnumerable<Categoria> Categorias()
        //{
        //    return _contexto.Categorias.ToList();
        //}

    }
}
