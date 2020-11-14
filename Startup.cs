using LanchesMac.Context;
using LanchesMac.Models;
using LanchesMac.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LanchesMac
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllersWithViews();
            // services.AddDbContext<AppDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("base")));
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(Util.GetConnectionString("base")));


            /// <summary>
            // configurando o servi�o de identifica��o de usuario 
            // passando o contexto da aplica��o
            /// </summary>
            services.AddIdentity<IdentityUser, IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();


            //servi�o par acesso negado.
            services.ConfigureApplicationCookie(options => options.AccessDeniedPath = "/Home/AcessDanied");


            //Registrando como servi�o minhas interfaces pra ser usado nos controles... 
            //specified = especificado  / transient = transitorio
            services.AddTransient<ICategoriaRepository, CategoriaRepository>();  // =>transient , significa que o objeto vai ser criado toda vez que for chamado, criando um novo objeto desse servico.
            services.AddTransient<ILancheRepository, LancheRepository>();
            services.AddTransient<IPedidoRepository, PedidoRepository>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();   //=> AddSingleton   => � instanciado uma �nica vez... ou seja todas as chamadas "requisi��es" obt�m o mesmo objeto.

            //cria um objeto Scoped, ou seja um objeto que esta associado a requisi��o
            //isso significa que se duas pessoas solicitarem o objeto CarrinhoCompra ao  mesmo tempo
            //elas v�o obter inst�ncias diferentes
            services.AddScoped(sp => CarrinhoCompra.GetCarrinho(sp));             //=> � criado instancia diferentes do objeto pra cada requisi��o...
           
            
            services.AddMemoryCache();
            services.AddSession();
            //services.AddMvc();  eu nao tinha
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();

            /// <summary>
            // Habilitando o dientity
            /// </summary>
            app.UseAuthentication();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {

                endpoints.MapControllerRoute(
                   name: "AdminArea",
                   pattern: "{area:exists}/{controller=Admin}/{action=Index}/{id?}");


                endpoints.MapControllerRoute(
                  name: "categoriaFiltro",
                  pattern: "Lanche/{action}/{categoria?}"
                  , defaults:new { Controller = "Lanche", action = "List"});


                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
