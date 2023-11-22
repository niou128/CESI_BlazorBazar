using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using WebApp;
using WebApp.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

var baseAddress = new Uri("https://localhost:32788");
var httpHandler = new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler());
builder.Services.AddScoped(sp => GrpcChannel.ForAddress(baseAddress, new GrpcChannelOptions { HttpHandler = httpHandler }));

builder.Services.AddScoped(services =>
{
    var baseAddress = new Uri("https://localhost:32788");
    var httpHandler = new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler());
    var grpcChannel = GrpcChannel.ForAddress(baseAddress, new GrpcChannelOptions { HttpHandler = httpHandler });
    return new OrderServiceClient(grpcChannel);
});

builder.Services.AddScoped<UserServiceClient>();
builder.Services.AddScoped<ProductServiceClient>();
builder.Services.AddScoped<OrderServiceClient>();

await builder.Build().RunAsync();
