using Bank.Client;
using Bank.Client.Services;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient<IUserDataService, UserDataService>(client => client.BaseAddress = new Uri("https://deploybankapp.azurewebsites.net/"));

builder.Services.AddHttpClient<IWalletService, WalletService>(client => client.BaseAddress = new Uri("https://deploybankapp.azurewebsites.net/"));

builder.Services.AddHttpClient<ISymbolAcService, SymbolAcService>(client => client.BaseAddress = new Uri("https://deploybankapp.azurewebsites.net/"));


builder.Services.AddBlazoredLocalStorage();

await builder.Build().RunAsync();