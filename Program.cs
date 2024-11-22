using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using PupaaS.Client;
using PupaaS.Client.Services;
using PupaaS.Client.Services.Mock;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddMudServices();

builder.Services.AddOidcAuthentication(options =>
{
    builder.Configuration.Bind("GoogleOIDC", options.ProviderOptions);
    // options.ProviderOptions.DefaultScopes.Add("{API SCOPE URI}");
});

builder.Services.AddSingleton<IPupusaService, MockPupusaService>();

await builder.Build().RunAsync();