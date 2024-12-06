using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using TestWithBasicAuth.Auth;

namespace TestWithBasicAuth
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
      
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            // Register dependencies
            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddAuthorizationCore();
            builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();

            // Register CustomAuthorizationMessageHandler
            builder.Services.AddScoped<CustomAuthorizationMessageHandler>();

            // Register HttpClient with CustomAuthorizationMessageHandler
            builder.Services.AddHttpClient("SecureApi", client =>
            {
                client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
            }).AddHttpMessageHandler<CustomAuthorizationMessageHandler>();

            // Provide a default HttpClient for non-secure API calls
            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            await builder.Build().RunAsync();
        }

    }
}

