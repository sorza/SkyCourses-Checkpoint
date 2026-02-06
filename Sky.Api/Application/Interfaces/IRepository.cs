namespace Sky.Api.Application.Interfaces
{
    public interface IRepository<T>
    {
        Task<IEnumerable<T>?> GetAllAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<T>?> GetAllAsync(Func<T, bool> predicate, CancellationToken cancellationToken = default);
        Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<T?> GetAsync(Func<T, bool> predicate, CancellationToken cancellationToken = default);
        Task CreateAsync(T entidade, CancellationToken cancellationToken = default);
        Task UpdateAsync(T entidade, CancellationToken cancellationToken = default);
        Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
