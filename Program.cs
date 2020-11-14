using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LanchesMac.Data;
using LanchesMac.Extensao;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace LanchesMac
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //criamos um metodo de extensão
            CreateHostBuilder(args)
           .Build()
           .CreateAdminRole()
           .Run();

        }

        public static IHostBuilder CreateHostBuilder(string[] args)=>
            Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(WeBuilder =>
            {
                WeBuilder.UseStartup<Startup>();
            });
    }
}
