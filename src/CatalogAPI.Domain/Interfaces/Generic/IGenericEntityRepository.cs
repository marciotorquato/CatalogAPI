using System.Linq.Expressions;

namespace CatalogAPI.Domain.Interfaces.Generic;

public interface IGenericEntityRepository<T>
{
    bool Exists(Expression<Func<T, bool>> predicate);
    void Delete(T entity);
    Task<bool> DeleteById(Guid id);
    Task<T> Insert(T entity, CancellationToken cancellationToken = default);
    (T entity, bool success) Update(T entity);
    IQueryable<T> Get();
    T GetById(Guid id);
    T GetByIdInt(int id);
    List<T> GetContainsId(Expression<Func<T, bool>> predicate);
    int LastId(Expression<Func<T, int>> predicate);
    Task<List<T>> ListarPaginacao(int take, int skip);
    Task<List<T>> BuscarPorIdsAsync<TKey>(IEnumerable<TKey> ids, Expression<Func<T, TKey>> keySelector);
}