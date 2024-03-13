// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WPR.Repositories.EntityFramework.Integer;

Console.WriteLine("Hello, World!");

var builder = new HostBuilder();

builder.ConfigureServices(sc =>
{
    sc.AddDbRepositories();
});

var host = builder.Build();

host.Services.GetKeyedServices()

Console.ReadKey();

class 