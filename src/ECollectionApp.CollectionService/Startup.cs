using ECollectionApp.AspNetCore.Microservice;
using ECollectionApp.AspNetCore.Patch.Converters;
using ECollectionApp.CollectionService.Authorization;
using ECollectionApp.CollectionService.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace ECollectionApp.CollectionService
{
    public class Startup : ECollectionAppServiceStartup
    {
        public Startup(IConfiguration configuration) : base(configuration) { }

        protected override string ServiceScope => "collections";

        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ECollectionApp.CollectionService", Version = "v1" });
            });

            services.AddSingleton<IAuthorizationHandler, CollectionAuthorizationHandler>();

            services.AddChangePatcher();

            services.AddDbContext<CollectionDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("CollectionDb")));

            services.Configure<JsonOptions>(options => {
                options.JsonSerializerOptions.Converters.Add(new PatchJsonTextConverter());
                options.JsonSerializerOptions.Converters.Add(new ObjectJsonTextConverter());
            });
        }

        public override void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            base.Configure(app, env);
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ECollectionApp.CollectionService v1"));
            }
        }
    }
}
