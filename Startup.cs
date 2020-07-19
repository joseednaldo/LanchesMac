using LanchesMac.Context;
using LanchesMac.Models;
using LanchesMac.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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

            //Registrando como serviço minhas interfaces pra ser usado nos controles... 
            //specified = especificado  / transient = transitorio
            services.AddTransient<ICategoriaRepository, CategoriaRepository>();
            services.AddTransient<ILancheRepository, LancheRepository>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();   //=> AddSingleton   => é instanciado uma unica vez...
            services.AddScoped(sp => CarrinhoCompra.GetCarrinho(sp));             //=> é criado instancia diferentes do objeto pra cada requisição...
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
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
