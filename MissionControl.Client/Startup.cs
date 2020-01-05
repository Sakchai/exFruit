using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.DependencyInjection;
using MissionControl.Client.Util;
using System;
using System.Net.Http;

namespace MissionControl.Client
{
    public class Startup
    {
        public const string BackendUrl = "http://localhost:64170";
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthorizationCore();
            services.AddTokenAuthenticationStateProvider();
            services.AddScoped(serviceProvider =>
            {
                var httpClient = new HttpClient { BaseAddress = new Uri(BackendUrl) };
                return new ReceptionState(httpClient);
            });
            services.AddScoped(serviceProvider =>
            {
                var httpClient = new HttpClient { BaseAddress = new Uri(BackendUrl) };
                return new PurchaseState(httpClient);
            });
            services.AddScoped<ReceptionState>();
            services.AddScoped<PurchaseState>();
        }

        public void Configure(IComponentsApplicationBuilder app)
        {
            app.AddComponent<App>("app");
        }
    }
}
