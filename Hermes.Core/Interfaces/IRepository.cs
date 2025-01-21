using System.Linq.Expressions;

namespace Hermes.Core.Interfaces;

public interface IRepository<T>
{
    IQueryable<T> FindAll();

    IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);

    T Create(T entity);

    void Update(T entity);

    void Delete(T entity);
}