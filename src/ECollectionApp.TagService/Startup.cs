using ECollectionApp.AspNetCore.Microservice;
using ECollectionApp.TagService.Data;
using ECollectionApp.TagService.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace ECollectionApp.TagService
{
    public class Startup : ECollectionAppServiceStartup
    {
        public Startup(IConfiguration configuration) : base(configuration) { }

        protected override string ServiceScope => "tags";

        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ECollectionApp.TagService", Version = "v1" });
            });

            services.AddDbContext<TagDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("TagDb")));

            services.Configure<JsonOptions>(options => {
                options.JsonSerializerOptions.Converters.Add(new TagJsonConverter());
            });
        }

        public override void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            base.Configure(app, env);
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ECollectionApp.TagService v1"));
            }
        }
    }
}
