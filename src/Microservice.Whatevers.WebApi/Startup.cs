using System;
using System.Net;
using System.Net.Http;
using FluentValidation.AspNetCore;
using Microservice.Whatevers.Repositories.Contexts;
using Microservice.Whatevers.Repositories.IoC;
using Microservice.Whatevers.Services.IoC;
using Microservice.Whatevers.Services.Validators;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Polly;
using Polly.Extensions.Http;

namespace Microservice.Whatevers.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, WhateverContext whateverContext)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => endpoints.MapControllers());

            whateverContext.Database.Migrate();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddApiVersioning();

            services.AddMvcCore(options => options.SuppressAsyncSuffixInActionNames = false)
               .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<WhateverModelValidator>());

            services.AddHttpClient("google", c => c.BaseAddress = new Uri(Configuration["UrlBaseGoogle"]))
               .AddPolicyHandler(GetRetryPolicy());

            services.AddDbContext(Configuration);
            services.AddRepository();
            services.AddAutoMapper();
            services.AddServices();
        }

        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
            => HttpPolicyExtensions.HandleTransientHttpError()
               .OrResult(msg => msg.StatusCode == HttpStatusCode.NotFound)
               .WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
    }
}