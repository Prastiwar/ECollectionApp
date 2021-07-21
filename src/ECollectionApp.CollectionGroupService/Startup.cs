using ECollectionApp.AspNetCore.Microservice;
using ECollectionApp.CollectionGroupService.Authorization;
using ECollectionApp.CollectionGroupService.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace ECollectionApp.CollectionGroupService
{
    public class Startup : ECollectionAppServiceStartup
    {
        public Startup(IConfiguration configuration) : base(configuration) { }

        protected override string ServiceScope => "collections";

        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ECollectionApp.CollectionGroupService", Version = "v1" });
            });
            services.AddSingleton<IAuthorizationHandler, CollectionGroupAuthorizationHandler>();
            services.AddDbContext<CollectionGroupDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("CollectionGroupDb")));
        }

        public override void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            base.Configure(app, env);
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ECollectionApp.CollectionGroupService v1"));
            }
        }
    }
}
