using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Neo4jClient;
using Pages.API.Infrastructure;
using Pages.API.Infrastructure.Filters;
using Pages.API.Infrastructure.Repositories;
using Swashbuckle.AspNetCore.Swagger;

namespace Pages.API
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
            services
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddControllersAsServices();

            services.Configure<PagesSettings>(Configuration);

            // Add framework services.
            services.AddSwaggerGen(options =>
            {
                options.DescribeAllEnumsAsStrings();
                options.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info
                {
                    Title = "Pages HTTP API",
                    Version = "v1",
                    Description = "The Pages Service HTTP API",
                    TermsOfService = "Terms Of Service"
                });

                options.AddSecurityDefinition("oauth2", new OAuth2Scheme
                {
                    Type = "oauth2",
                    Flow = "implicit",
                    AuthorizationUrl = $"{Configuration.GetValue<string>("IdentityUrlExternal")}/connect/authorize",
                    TokenUrl = $"{Configuration.GetValue<string>("IdentityUrlExternal")}/connect/token",
                    Scopes = new Dictionary<string, string>()
                    {
                        { "pages", "Pages API" }
                    }
                });

                options.OperationFilter<AuthorizeCheckOperationFilter>();
            });

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                    .SetIsOriginAllowed((host) => true)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });

            services.AddSingleton<NeoServerConfiguration>(provider => {
                return NeoServerConfiguration
                    .GetConfiguration(new Uri(Configuration.GetValue<string>("Neo4jConnectionString")), "neo4j", "password");
            });

            services.AddSingleton<IGraphClientFactory, GraphClientFactory>();
            services.AddSingleton<PageDataContext, PageDataContext>();
            services.AddTransient<IPageDataRepository, PageDataRepository>();
            services.AddOptions();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            var pathBase = Configuration["PATH_BASE"];

            if (!string.IsNullOrEmpty(pathBase))
            {
                app.UsePathBase(pathBase);
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                // app.UseHsts();
            }

            //app.UseHttpsRedirection();

            app.UseMvcWithDefaultRoute();

            app.UseSwagger()
               .UseSwaggerUI(c =>
               {
                   c.SwaggerEndpoint($"{ (!string.IsNullOrEmpty(pathBase) ? pathBase : string.Empty) }/swagger/v1/swagger.json", "Pages.API V1");
                   c.OAuthClientId("pagesswaggerui");
                   c.OAuthAppName("Pages Swagger UI");
               });
        }
    }
}
