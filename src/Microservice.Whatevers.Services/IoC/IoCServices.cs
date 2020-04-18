using AutoMapper;
using Microservice.Whatevers.Services.Clients;
using Microservice.Whatevers.Services.Mapper;
using Microsoft.Extensions.DependencyInjection;

namespace Microservice.Whatevers.Services.IoC
{
    public static class IocServices
    {
        public static void Register(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(ModelToDomainProfile));
            services.AddScoped<IWhateverService, WhateverService>();
            services.AddScoped<IGoogleService, GoogleService>();
            services.AddScoped<IGoogleClient, GoogleClient>();
        }
    }
}