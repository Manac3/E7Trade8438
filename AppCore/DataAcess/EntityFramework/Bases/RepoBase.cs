using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.DataAcess.EntityFramework.Bases
{
    public abstract class RepoBase<TEntity> : IDisposable where TEntity : class, new() //Repository Pattern
        //veri erişim ile ilgili genel bir class yazıp bu veri erişim işlemlerini burdan sağlayacağız bu bir abstract class olmalı bundan miras alan clası newlecez
        //bu recordbase olmamalı çünkü id ara tablolarda olmaz bundan dolayı sadece class olmalı ve tüm tablolara ulaşabilmeliyiz
    {
        protected readonly DbContext _dbContext;

        protected RepoBase(DbContext dbContext)
        {
            _dbContext = dbContext;
        }
        //şimdi bu repository ile crud işlemlerini yapıcaz ve tüm entitylerde kullanabilicez
        public IQueryable<TEntity> Query(params Expression<Func<TEntity, object?>>[] entitiesToInclude) 
        {
            var query = _dbContext.Set<TEntity>().AsQueryable();
            foreach (var entityToInclude in entitiesToInclude)
            {
                query = query.Include(entityToInclude);
            }
            return query;
        }
        public IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object?>>[] entitiesToInclude)
        {
            var query = Query(entitiesToInclude);
            query = query.Where(predicate);
            return query;  
        }

        public void Dispose()
        {
            _dbContext?.Dispose(); //burda soru işaretiyle null olamaz dedik
            GC.SuppressFinalize(this);
        }
    }
}
