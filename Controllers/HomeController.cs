using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using LanchesMac.Models;
using LanchesMac.Repositories;
using LanchesMac.ViewModels;

namespace LanchesMac.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ILancheRepository _lancheRepository;

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}


        public HomeController(ILancheRepository lancheRepositiyr)
        {
            _lancheRepository = lancheRepositiyr;
        }
        public IActionResult Index()
        {
            var homeViewModel = new HomeViewModel
            {
                
                LanchePreferidos = _lancheRepository.LanchesPrefferido
               
            };
            return View(homeViewModel);
        }


    }
}
