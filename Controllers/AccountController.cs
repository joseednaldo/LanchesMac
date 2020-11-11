using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LanchesMac.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace LanchesMac.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _useManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController( UserManager<IdentityUser> useManager,
                                 SignInManager<IdentityUser> signInManager)
        {
            _useManager = useManager;
            _signInManager = signInManager;

        }

        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            return View(new LoginViewModel()
            {
                ReturnUrl = returnUrl
            });
        }


        [HttpPost]
        public async Task<IActionResult>Login(LoginViewModel loginVM)
        {

            //validando modelo recebido
            if (!ModelState.IsValid)
                return View(loginVM);

            var user = await _useManager.FindByNameAsync(loginVM.UserName);

            if (user!=null){

                var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, false, false);

                if (result.Succeeded)
                {
                    if (string.IsNullOrEmpty(loginVM.ReturnUrl))
                    {
                        return RedirectToAction("Index", "Home");
                    }

                    return RedirectToAction(loginVM.ReturnUrl);
                }
            }

            ModelState.AddModelError("", "Usuário invalido ou não localizado!!");
            return View(loginVM);
        }

        public IActionResult Register()
        {
            //apresenta formulario
            return View();
        }
        

        [HttpPost]
        [ValidateAntiForgeryToken] //Esse FILTRO indica que para executar esse metodo precisa ser autenticado com um token valido. EVITA ATAQUE DO TIPO CSRF
        public async Task<IActionResult>Register(LoginViewModel registeVM)
        {

            //recebe dados via MODE BIDEN PARA CADASTRO
            if (ModelState.IsValid)
            {
                var user = new IdentityUser()
                {
                    UserName = registeVM.UserName
                };

                var resultado = await _useManager.CreateAsync(user, registeVM.Password);

                if (resultado.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            return View(registeVM);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
