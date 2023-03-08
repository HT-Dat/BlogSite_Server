using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using DAL.Repositories.IRepositories;

namespace DAL.Repositories;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class, new()
{
    private readonly BlogSiteDbContext _blogSiteDbContext;
    public GenericRepository(BlogSiteDbContext blogSiteDbContext)
    {
        _blogSiteDbContext = blogSiteDbContext;
    }

    public async Task<TEntity> Add(TEntity entity)
    {
        await _blogSiteDbContext.AddAsync(entity);
        await _blogSiteDbContext.SaveChangesAsync();
        return entity;
    }

    public async Task<List<TEntity>> AddRange(List<TEntity> entity)
    {
        await _blogSiteDbContext.AddRangeAsync(entity);
        await _blogSiteDbContext.SaveChangesAsync();
        return entity;
    }

    public async Task<int> Delete(TEntity entity)
    {
        _ = _blogSiteDbContext.Remove(entity);
        return await _blogSiteDbContext.SaveChangesAsync();
    }

    public async Task<TEntity> Get(Expression<Func<TEntity, bool>> filter = null)
    {
        return await _blogSiteDbContext.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync(filter);
    }

    public async Task<List<TEntity>> GetList(Expression<Func<TEntity, bool>> filter = null)
    {
        return await (filter == null ? _blogSiteDbContext.Set<TEntity>().ToListAsync() : _blogSiteDbContext.Set<TEntity>().Where(filter).ToListAsync());
    }

    public async Task<TEntity> Update(TEntity entity)
    {
        _ = _blogSiteDbContext.Update(entity);
        await _blogSiteDbContext.SaveChangesAsync();
        return entity;
    }

    public async Task<List<TEntity>> UpdateRange(List<TEntity> entity)
    {
        _blogSiteDbContext.UpdateRange(entity);
        await _blogSiteDbContext.SaveChangesAsync();
        return entity;
    }
}
