using LanchesMac.Context;
using LanchesMac.Migrations;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LanchesMac.Models
{
    public class CarrinhoCompra
    {
        private readonly AppDbContext _context;

        public CarrinhoCompra(AppDbContext contexto)
        {
             _context = contexto;
        }
        public string CarrinhoCompraId { get; set; }
        public List<CarrinhoCompraItem> CarrinhoCompraItens { get; set; }

        public static CarrinhoCompra GetCarrinho(IServiceProvider services)
        {
            // define uma sessão acesando o contexto atual (tem que registrar o IServicesCollection)
            ISession session =
                services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;
            //? operador condicional...se eu nao tiver uma session retorna null ,se eu tiver uma sessioan ela é retornada =>HttpContext.Session.

            //obtem um serviço do tipo nosso contexto.
            var context = services.GetService<AppDbContext>();

            //obtem ou gera o Id carrinho
            string carrinhoId = session.GetString("CarrinhoId") ?? Guid.NewGuid().ToString();
            //se o carrinho existe ele é retornado o id do carrinho, caso o carrinho nao existe é retornado um codigo de 128 bytes....

            //atribui o id do carrinho na sessão
            session.SetString("Carrinho", carrinhoId);

            //retorna o carrinho com o contexto atual e o id atribuido ou obtido
            return new CarrinhoCompra(context)
            {
                CarrinhoCompraId = carrinhoId
            };
        
        
        }

        public void AdicionarAoCarrinho(Lanche lanche ,int quantidade)
        {
            var carrinhoCompraItem =
                _context.CarrinhoCompraItens.SingleOrDefault(
                    s=> s.Lanche.LancheId == lanche.LancheId && s.CarrinhoCompraId == CarrinhoCompraId
                    );

            //verifica se o carrinho existe e senão ecistir cria um 
            if(carrinhoCompraItem == null)
            {
                carrinhoCompraItem = new CarrinhoCompraItem
                {
                    CarrinhoCompraId = CarrinhoCompraId,
                    Lanche = lanche,
                    Quantidade = 1
                };
                _context.CarrinhoCompraItens.Add(carrinhoCompraItem);
            }
            else
            {
                // se existir o carrinho com o item então incrementa a quantidade.
                carrinhoCompraItem.Quantidade++;
            }
            _context.SaveChanges();

        }

        public int RemoverDoCarrinho(Lanche lanche)
        {
            var carrinhoCompraItem =
                _context.CarrinhoCompraItens.SingleOrDefault(
                        s => s.Lanche.LancheId == lanche.LancheId && s.CarrinhoCompraId == CarrinhoCompraId
                    );

            var quantidadeLocal = 0;

            if (carrinhoCompraItem != null)
            {
                if(carrinhoCompraItem.Quantidade > 1)
                {
                    carrinhoCompraItem.Quantidade--;
                    quantidadeLocal = carrinhoCompraItem.Quantidade;
                }
                else
                {
                    _context.CarrinhoCompraItens.Remove(carrinhoCompraItem);

                }
            }
            _context.SaveChanges();
            return quantidadeLocal;
        }

        public  void LimpaCarrinho()
        {
            var carrinhoItens = _context.CarrinhoCompraItens
                                    .Where(carrinho => carrinho.CarrinhoCompraId == CarrinhoCompraId);
            _context.CarrinhoCompraItens.RemoveRange(carrinhoItens);
            _context.SaveChanges();
        }

        public decimal GetCarrinhoCompraTotal()
        {
            try
            {
                return _context.CarrinhoCompraItens.Where(c => c.CarrinhoCompraId == CarrinhoCompraId)
               .Select(c => c.Lanche.Preco * c.Quantidade).Sum();
            }
            catch (Exception)
            {

                throw;
            }
        }
        public List<CarrinhoCompraItem> GetCarrinhoCompraItens()
        {
            return CarrinhoCompraItens ??
                (CarrinhoCompraItens =
                _context.CarrinhoCompraItens.Where(c => c.CarrinhoCompraId == CarrinhoCompraId)
                .Include(s => s.Lanche)
                .ToList());
        }

    }
}
