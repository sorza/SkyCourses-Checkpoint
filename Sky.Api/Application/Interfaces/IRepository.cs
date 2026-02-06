namespace Sky.Api.Application.Interfaces
{
    public interface IRepository<T>
    {
        Task<IEnumerable<T>?> GetAllAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<T>?> GetAllAsync(Func<T, bool> predicate, CancellationToken cancellationToken = default);
        Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<T?> GetByIdAsync(Guid id, Func<T, bool> predicate, CancellationToken cancellationToken = default);
        Task CreateAsync(T entidade, CancellationToken cancellationToken = default);
        Task UpdateAsync(T entidade, CancellationToken cancellationToken = default);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
