using CITADEL_NEXUS_PUBLIC;
using CITADEL_NEXUS_PUBLIC.Models;
using CITADEL_NEXUS_PUBLIC.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Options;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
var publicOracleOptions = builder.Configuration.GetSection(PublicOracleOptions.SectionName).Get<PublicOracleOptions>() ?? new PublicOracleOptions();
builder.Services.AddSingleton(Options.Create(publicOracleOptions));
builder.Services.AddMudServices();
builder.Services.AddScoped<SanityFilterService>();
builder.Services.AddScoped<PublicOracleClient>();

await builder.Build().RunAsync();
