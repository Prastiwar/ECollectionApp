using ECollectionApp.AspNetCore.Microservice.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace ECollectionApp.AspNetCore.Microservice
{
    public abstract class ECollectionAppServiceStartup
    {
        public ECollectionAppServiceStartup(IConfiguration configuration) => Configuration = configuration;

        public IConfiguration Configuration { get; }

        protected abstract string ServiceScope { get; }

        public virtual void ConfigureServices(IServiceCollection services)
        {
            string authority = $"https://{Configuration["Auth0:Domain"]}/";
            services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options => {
                options.Authority = authority;
                options.Audience = Configuration["Auth0:Audience"];
                options.TokenValidationParameters = new TokenValidationParameters {
                    NameClaimType = ClaimTypes.NameIdentifier
                };
            });

            services.AddAuthorization(options => {
                AuthorizationPolicy policy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
                                                      .AddRequirements(new HasScopeRequirement(ServiceScope, authority))
                                                      .RequireClaim(ECollectionAppClaims.Account_Id)
                                                      .Build();
                options.DefaultPolicy = policy;
            });

            services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();

            services.AddControllers();
        }

        public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        }
    }
}
