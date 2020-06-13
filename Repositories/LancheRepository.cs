using LanchesMac.Context;
using LanchesMac.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LanchesMac.Repositories
{
    public class LancheRepository : ILancheRepository
    {
        private readonly AppDbContext _contexto;

        public LancheRepository(AppDbContext contexto)
        {
            _contexto = contexto;
        }

        public IEnumerable<Lanche> Lanches()
        {
            var lanche = _contexto.Lanches.ToList();

            return lanche.ToList();
        }

        public IEnumerable<Lanche> LanchesPrefferido => _contexto.Lanches.Where(lp=> lp.IsLanchePreferido).Include(c=>c.Categoria);

        //traz os lanches e categorias.metodo include.
        IEnumerable<Lanche> ILancheRepository.Lanches => _contexto.Lanches.Include(c=>c.Categoria);

        public Lanche GetLancheById(int lancheId)
        {
            return _contexto.Lanches.FirstOrDefault(l => l.LancheId == lancheId);
        }
    }
}
