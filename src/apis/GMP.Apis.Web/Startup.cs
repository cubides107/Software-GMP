using GMP.Infrastructure.Startup;
using GMP.Researchs.Application.UsersEventsHandlers;
using GMP.Researchs.Infrastructure.Startup;
using GMP.Users.Infrastructure.Startup;
using GMP.Users.IntegrationEvent;
using GMP.Users.IntegrationEvent.EventAccessRoot;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMP.Apis.Web
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
            //servicios de controller base
            services.AddControllers();

            //servicios de swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "GMP.Apis.Web", Version = "v1" });
            });

            //configuracion api para la autentificacion por jwt token
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                 options.TokenValidationParameters = new TokenValidationParameters
                 {
                     ValidateIssuer = true,
                     ValidateAudience = true,
                     ValidateLifetime = true,
                     ValidateIssuerSigningKey = true,
                     ValidIssuer = Configuration["DOMINIO_APP"],
                     ValidAudience = Configuration["DOMINIO_APP"],
                     IssuerSigningKey = new SymmetricSecurityKey(
                     Encoding.UTF8.GetBytes(Configuration["CLAVE_SECRETA"])),
                     ClockSkew = TimeSpan.Zero
                 });

            //iniciar y configurar los modulos
            GMPStartup.SetUp(services, this.Configuration);
            UsersStartup.SetUp(services, this.Configuration);
            ResearchsStartup.Setup(services, this.Configuration);

            //suscribirse a eventos "in memory even bus JKAN" (aun no hay nadie que se sbscriba)
            services.AddEventBus(builder => {
                builder.AddInMemoryEventBus(subscriber =>
                {
                    //1.evento 2.manejador
                    subscriber.Subscribe<AccessRootEvent, AccessRootEventHandler>();
                    subscriber.Subscribe<InternalRegisterExternalEvent, InternalRegisterExternalEventHandler> ();
                    subscriber.Subscribe<RootRegisterUserExternalEvent, RootRegisterUserExternalEventHandler> ();
                    subscriber.Subscribe<RootRegisterUserInternalEvent, RootRegisterUserInternalEventHandler> ();
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "GMP.Apis.Web v1"));
            }

            //app.UseHttpsRedirection(); se comenta el redireccionamiento https para que el arduino pueda funcionar

            //CORS
            app.UseCors(options =>
            {
                options.WithOrigins("*");
                options.AllowAnyMethod();
                options.AllowAnyHeader();
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
