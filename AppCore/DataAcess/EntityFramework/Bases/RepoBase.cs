using AppCore.Records.Bases;
using AppCore.Results.Bases;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.DataAcess.EntityFramework.Bases
{
    public abstract class RepoBase<TEntity> : IDisposable where TEntity : RecordBase, new() //Repository Pattern
        //veri erişim ile ilgili genel bir class yazıp bu veri erişim işlemlerini burdan sağlayacağız bu bir abstract class olmalı bundan miras alan clası newlecez
        //bu recordbase olmamalı çünkü id ara tablolarda olmaz bundan dolayı sadece class olmalı ve tüm tablolara ulaşabilmeliyiz
    {
        public DbContext DbContext { get; set; }

        protected RepoBase(DbContext dbContext)
        {
            DbContext = dbContext;
        }
        //şimdi bu repository ile crud işlemlerini yapıcaz ve tüm entitylerde kullanabilicez
        public virtual IQueryable<TEntity> Query(params Expression<Func<TEntity, object?>>[] entitiesToInclude)
        {
            var query = DbContext.Set<TEntity>().AsQueryable();
            foreach (var entityToInclude in entitiesToInclude)
            {
                query = query.Include(entityToInclude);
            }
            return query;
        }
        public virtual IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object?>>[] entitiesToInclude)
        {
            var query = Query(entitiesToInclude);
            query = query.Where(predicate);
            return query;
        }

        public virtual void Add(TEntity entity, bool save = true)
        {
            DbContext.Set<TEntity>().Add(entity);
            if (save)
                Save();
        }

        public virtual void Update(TEntity entity, bool save = true)
        {
            DbContext.Set<TEntity>().Update(entity);
            if (save)
                Save();
        }

        public virtual void Delete(TEntity entity, bool save = true) //kaydı silme
        {
            DbContext.Set<TEntity>().Remove(entity);
            if (save)
                Save();
        }

        public virtual void Delete(int id, bool save=true) //id'ye göre silme
        {
            //var entity = DbContext.Set<TEntity>().Find(id);

            /*
            {
                var entity = Query().First(e => e.Id = id);
                var entity = Query().FirstOrDefault(e => e.Id = id);
            }
            yukardakilerle de ilkine ulaşmayı kullandık yine aşağıdakilerletemelolarak farkları aynı 
            bunlara ek olarak sonuncuya da ulaşmak mümkün bunun için last ve lastordefault kullanırız
            

            bir diğer sorgulama metodu where olanı kullanabiliriz where bize liste koleksiyon döner dolayısıyla tolist deriz ve where ile filtreleme yaparsak listeden bakarız
            

            
            var entity = Query().Single(e => e.Id == id);
            single tek bir kayıt var mı diye bakıyor ve birden fazla aynı kayıt varsa hata fıraltır ve bizde aa hata var deyip düzeltme yapabiliriz
            single belirtilen kayıda uyan yoksa exception fırlatıp programı patlatıyor
            
            burda ise koşula uyan kayıt varsa onu döner yoksa da default olaraknulldöner 
            or defaultu kullanmak daha mantıklı çünkü null döner hata atmaz ve böylece kayıt olmadığını anlarız
            */
            var entity = Query().SingleOrDefault(e => e.Id == id);
            Delete(entity, save);


        }


        public virtual int Save()
        {
            try
            {
                return DbContext.SaveChanges();
            }
            catch (Exception exc)
            {
                //istenirse hata loglama kodları yazılabilir
                throw exc;
            }
        }

        public void Dispose()
        {
            DbContext?.Dispose(); //burda soru işaretiyle null olamaz dedik
            GC.SuppressFinalize(this);
        }
    }
}
