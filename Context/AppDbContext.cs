using LanchesMac.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LanchesMac.Context
{

    //Mudamos o contexto para herda  de IdentityDbContext com a classe que vai gerenciar os usuarios e criar as tabelas e
    //fazer o  gerenciamento.
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options ): base(options)
        {}

        /*
            Quando você cria um contexto para o EF e indica as classes do mapeamento, 
            basicamente diz a ele que todos os objetos deverão ser rastreados, ou seja, 
            o simples fato de você criar um objeto ou ler a partir do contexto, 
            coloca este objeto sobre o controle do EF.
         */
        public DbSet<Lanche> Lanches { get; set; }
        public DbSet <Categoria> Categorias { get; set; }
        public DbSet<CarrinhoCompraItem>CarrinhoCompraItens { get; set; }

        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<PedidoDetalhe> PedidoDetalhes { get; set; }
    }
}
