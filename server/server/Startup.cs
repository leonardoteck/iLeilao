using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using server.Controllers;

namespace server
{
    public class Startup
    {
        IConfigurationRoot Configuration;
        private const string SecretKey = "F9bNO053oHGlT7ChEmBGRfohNHqqH2xzhtD/59C8t08Kq0iQu5E1K8PTR00JMq28g9Imbmo47U";
        private readonly SymmetricSecurityKey _signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecretKey));

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
            services.AddOptions();
            services.AddCors();
            services.AddAuthorization();
            //services.AddSingleton<CronometroController>();
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
            })
            .AddEntityFrameworkStores<LeilaoContext>();

            // Add framework services.
            services.AddMvc();

            var jwtAppSettingOptions = Configuration.GetSection(nameof(JwtIssuerOptions));

            services.Configure<JwtIssuerOptions>(options =>
            {
                options.Issuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                options.Audience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)];
                options.SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256);
            });           
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //Permite que requisições sejam feitas de outras fontes
            app.UseCors(builder =>
                builder.AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowAnyOrigin()
                    .AllowCredentials()
            );

            //Configuração da autenticação via Token
            var jwtAppSettingOptions = Configuration.GetSection(nameof(JwtIssuerOptions));
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)],

                ValidateAudience = true,
                ValidAudience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)],

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _signingKey,

                RequireExpirationTime = true,
                ValidateLifetime = true,

                ClockSkew = TimeSpan.Zero
            };

            //fala que a autenticação será feita via JWT
            app.UseJwtBearerAuthentication(new JwtBearerOptions
            {
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                TokenValidationParameters = tokenValidationParameters
            });

            
            
            app.UseMvcWithDefaultRoute();
            

            if (env.IsDevelopment())
            {
                app.UseExceptionHandler(
                 options => {
                     options.Run(
                     async context =>
                     {
                         context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                         context.Response.ContentType = "text/html";
                         var ex = context.Features.Get<IExceptionHandlerFeature>();
                         if (ex != null)
                         {
                             var err = $"<h1>Error: {ex.Error.Message}</h1>{ex.Error.StackTrace }";
                             await context.Response.WriteAsync(err).ConfigureAwait(false);
                         }
                     });
                 });
            }
        }
    }
}
