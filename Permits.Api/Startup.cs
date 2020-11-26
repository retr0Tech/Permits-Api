using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Permits.Api.IoC;
using Permits.BL.IoC;
using Permits.Core.IoC;
using Permits.Model.Contexts.Permits;
using Permits.Model.IoC;
using AutoMapper;
using System;
using Permits.Core.Models;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.OData.Formatter;
using System.Linq;
using Microsoft.Net.Http.Headers;
using Permits.Api.Filters;
using FluentValidation.AspNetCore;
using Permits.BL.Validators;
using Microsoft.AspNetCore.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

namespace Permits.Api
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

            #region CORS
            //var appSettings = Configuration.GetSection("AppSettings").Get<AppSettings>();
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllPolicy", builder =>
                {
                    builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });
            #endregion

            #region IoC Registry
            services.AddCoreRegistry();
            services.AddModelRegistry();
            services.AddBlRegistry();
            services.AddApiRegistry();
            #endregion

            services.AddMvcCore(options =>
            {
                foreach (var outputFormatter in options.OutputFormatters.OfType<ODataOutputFormatter>().Where(_ => _.SupportedMediaTypes.Count == 0))
                {
                    outputFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/prs.odatatestxx-odata"));
                }
                foreach (var inputFormatter in options.InputFormatters.OfType<ODataInputFormatter>().Where(_ => _.SupportedMediaTypes.Count == 0))
                {
                    inputFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/prs.odatatestxx-odata"));
                }
            });

            #region ContextConfiguration
            string myAppDbContextConnection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<PermitsDbContext>(op => op.UseSqlServer(myAppDbContextConnection, x => x.UseNetTopologySuite()), ServiceLifetime.Scoped);
            #endregion


            services.AddMvc(o => {
                o.Filters.Add<ValidationHttpParametersFilter>();
                o.EnableEndpointRouting = false;
            })
            .AddNewtonsoftJson(options => {
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            })
            .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<KeyValueValidator>());

            #region Adding External Libs
            //Register AutoMapper
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Permits API", Version = "v1" });
            });
            #endregion


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Permits API V1");
                c.RoutePrefix = string.Empty;
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();

            app.UseCors("AllowAllPolicy");
            app.UseStaticFiles();
            app.UseHttpsRedirection();
            app.UseMvc(b =>
            {
                b.Select().Expand().Filter().OrderBy().MaxTop(100).Count();
                b.EnableDependencyInjection();
            });
        }
    }
}
