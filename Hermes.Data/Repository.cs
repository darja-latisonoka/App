using System.Linq.Expressions;
using Hermes.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hermes.Data;

public class Repository<T>(HermesDbContext dbContext) : IRepository<T>
    where T : class
{
    private readonly DbSet<T> _table = dbContext.Set<T>();

    public IQueryable<T> FindAll()
    {
        return dbContext.Set<T>();
    }

    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
    {
        return dbContext.Set<T>().Where(expression);
    }

    public T Create(T entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        _table.Add(entity);

        dbContext.SaveChanges();

        return entity;
    }

    public void Update(T entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        dbContext.SaveChanges();
    }

    public void Delete(T entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        _table.Remove(entity);

        dbContext.SaveChanges();
    }
}