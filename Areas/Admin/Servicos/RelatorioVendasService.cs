using LanchesMac.Context;
using LanchesMac.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LanchesMac.Areas.Admin.Servicos
{
    public class RelatorioVendasService
    {
        private readonly AppDbContext contexto;

        public RelatorioVendasService(AppDbContext _contexto)
        {
            contexto = _contexto;
        }

        public async Task<List<Pedido>> FindByDateAsync(DateTime? minDate, DateTime? maxDate)
        {
            var resultado = from obj in contexto.Pedidos 
                            select obj;


            if (minDate.HasValue)
            {
                resultado = resultado.Where(x => x.PedidoEnviado >= minDate.Value);
            }

            if (maxDate.HasValue)
            {
                resultado = resultado.Where(x => x.PedidoEnviado <= maxDate.Value);
            }

            return await resultado
                .Include(l => l.PedidoItens)  //acrescentando a tabela pedidoitens
                .ThenInclude(l => l.Lanche)  // acrescentando a tabela lanche
                .OrderByDescending(x => x.PedidoEnviado)
                .ToListAsync();

        }
    }
}
