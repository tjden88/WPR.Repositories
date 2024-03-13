// See https://aka.ms/new-console-template for more information

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WPR.Repositories.Base.Entities;
using WPR.Repositories.EntityFramework.Integer;
using WPR.Repositories.Integer;

Console.WriteLine("Hello, World!");

var builder = new HostBuilder();

builder.ConfigureServices(sc =>
{
    sc.AddDbRepositories();
    sc.AddDbContext<Db>();
});

var host = builder.Build();

var scope = host.Services.CreateScope();

var repository = scope.ServiceProvider.GetRequiredService<IRepository<Ent>>();
var repository2 = scope.ServiceProvider.GetRequiredService<INamedRepository<Ent>>();
var repository3 = scope.ServiceProvider.GetRequiredService<IDeletedRepository<Ent>>();

Console.ReadKey();


public sealed class Db : DbContext
{

    public DbSet<Ent> Ents { get; set; }

    public Db()
    {
        Database.Migrate();
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Test.db;");
    }
}

public class Ent : NamedEntity, IDeletedEntity
{
    public bool IsDeleted
    {
        get => throw new NotImplementedException();
        set => throw new NotImplementedException();
    }
}