using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Diagnostics;

namespace server
{
    public class Startup
    {
        IConfigurationRoot Configuration;

        public Startup(IHostingEnvironment env)
        {
             Configuration = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json")                
                .Build();
        }

        

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Configuração da Entity Framework + PostgreSQL
            services.AddEntityFrameworkNpgsql()
                .AddDbContext<LeilaoContext>(
                    options => options.UseNpgsql(
                        Configuration.GetConnectionString("LeilaoBD")));

            //Configuração do Identity
            services.AddIdentity<Usuario, IdentityRole>(opts =>
            {
                //Regra de email
                opts.User.RequireUniqueEmail = true;

                //Regras para as senhas
                opts.Password.RequireNonAlphanumeric = false;
                opts.Password.RequireUppercase = false;
                opts.Password.RequireLowercase = false;
                opts.Password.RequiredLength = 1;
            });
            // Add framework services.
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            //Permite que requisições sejam feitas de outras fontes
            app.UseCors(builder =>
                builder.AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowAnyOrigin()
                    .AllowCredentials()
            );

            app.UseMvcWithDefaultRoute();
        }
    }
}
