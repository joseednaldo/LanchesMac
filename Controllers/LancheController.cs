using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LanchesMac.Repositories;
using LanchesMac.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LanchesMac.Controllers
{
    public class LancheController : Controller
    {

        private readonly ILancheRepository _lancheRepository;
        private readonly ICategoriaRepository _categoriaRepository;

        public LancheController(ILancheRepository lancheRepository,
            ICategoriaRepository categoriaRepository)
        {
            //injeção de pendencia das interfaces...
            // quando o controlador for chamar ILancheRepository , como eu tenho um serviço registrado pra ele , vamos recuperar a instancia da implementação da interface => "lancheRepository"
            _lancheRepository = lancheRepository;
            _categoriaRepository = categoriaRepository;

        }

        public IActionResult List()
        {
            /*
             * 
                COMO POSSO PASSAR INFORMAÇÕES PRA VIEW VISUALIZAÇÃO...
                ViewBag.LANCHE = "Lanches";
                ViewData["Categoria"] = "Categoria";
             */

            ViewBag.LANCHE = "Lanches";
            ViewData["Categoria"] = "Categoria";

            /*Recuperando dados baseado no modelo
            var lanches = _lancheRepository.Lanches;
            return View(lanches); // VIEW TIPADA
            */

            //Recuperando dados baseado na viewmodel.
            var lancheslistViewModel = new LancheListViewModel();
            lancheslistViewModel.Lanches = _lancheRepository.Lanches;
            lancheslistViewModel.CategoriaAtual = "Categoria atual";
            return View(lancheslistViewModel);
        }

    }
}