using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using SalesWebMVC.Data;
using SalesWebMVC.Services;

namespace SalesWebMVC
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddDbContext<SalesWebMVCContext>(options =>
            options.UseMySql(Configuration.GetConnectionString("SalesWebMVCContext"), builder =>
            builder.MigrationsAssembly("SalesWebMVC")));

            /*Adicionando o seeding service na injeção de dependência*/
            services.AddScoped<SeedingService>();
            services.AddScoped<SellerService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.

        /*Vamos chamar a operação seed, para popular o banco, dentro do método Configure que permite outros
         parâmetros.*/
        /*Se você adicionar um parâmetro no método Configure e a classe desse parâmetro estiver registrada
         no sistema de injeção de dependência da aplicação, automaticamente vai ser resolvido uma instância
        desse objeto.*/
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, SeedingService seedingService)
        {
            //Perfil de desenvolvimento
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //No perfil de produção eu vou rodar o SeedingService
                seedingService.Seed(); //Então irá popular a base de dados para testes
            }

            //Aplicação publicada
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
