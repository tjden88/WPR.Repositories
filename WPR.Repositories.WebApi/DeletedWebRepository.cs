using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace WPR.Repositories.WebApi;

public class DeletedWebRepository<T> : WebRepository<T>, IDeletedRepository<T> where T : Entity, IDeletedEntity, new()
{
    public DeletedWebRepository(HttpClient Client, ILogger<WebRepository<T>> Logger) : base(Client, $"api/Deleted{typeof(T).Name}", Logger) { }


    public virtual async Task<T> RestoreAsync(int id, CancellationToken Cancel = default) => await GetAsync<T>($"{Address}/restore/{id}", Cancel).ConfigureAwait(false);
}