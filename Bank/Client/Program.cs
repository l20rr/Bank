using Bank.Client;
using Bank.Client.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient<IUserDataService, UserDataService>(client => client.BaseAddress = new Uri("https://localhost:44340/"));

builder.Services.AddHttpClient<IWalletService, WalletService>(client => client.BaseAddress = new Uri("https://localhost:44340/"));

builder.Services.AddHttpClient<ISymbolAcService, SymbolAcService>(client => client.BaseAddress = new Uri("https://localhost:44340/"));

await builder.Build().RunAsync();