using System.Linq.Expressions;

namespace CatalogAPI.Domain.Interfaces.Generic;

public interface IGenericServices<T>
{
    bool Exists(Expression<Func<T, bool>> predicate);
    void Delete(T entity);
    Task<bool> DeleteById(Guid id);
    Task<T> Insert(T entity);
    Task<(T entity, bool success)> Update(T entity);
    IQueryable<T> Get();
    T GetById(Guid id);
    T GetByIdInt(int id);
    List<T> GetContainsId(Expression<Func<T, bool>> predicate);
    int LastId(Expression<Func<T, int>> predicate);
    Task<List<T>> ListarPaginacao(int take, int skip);
}
