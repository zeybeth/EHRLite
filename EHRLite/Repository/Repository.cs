using EHRLite.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EHRLite.Repository
{
    // IRepository arayüzünü (Interface) implemente ediyoruz
    public class Repository<T> : IRepository<T> where T : class
    {
        // Veritabanı bağlantısını (DbContext) buraya alıyoruz
        private readonly ApplicationDbContext _db;

        // Üzerinde çalışacağımız tabloyu temsil eden değişken
        internal DbSet<T> dbSet;

        // Constructor (Yapıcı Metot): DbContext'i içeri alır
        public Repository(ApplicationDbContext db)
        {
            _db = db;
            this.dbSet = _db.Set<T>();
        }

        // Ekleme İşlemi
        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        // Tek bir kayıt getirme (Filtreleyerek)
        public T Get(Expression<Func<T, bool>> filter)
        {
            IQueryable<T> query = dbSet;
            query = query.Where(filter);
            return query.FirstOrDefault();
        }
        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null)
        {
            IQueryable<T> query = dbSet;

            // Eğer filtre varsa uygula (Örn: HastaId == 5)
            if (filter != null)
            {
                query = query.Where(filter);
            }

            // Eğer tablo birleştirme varsa uygula (Örn: "Patient")
            if (includeProperties != null)
            {
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }

            return query.ToList();
        }

        // Silme İşlemi
        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }

        // Toplu Silme İşlemi
        public void RemoveRange(IEnumerable<T> entities)
        {
            dbSet.RemoveRange(entities);
        }
    }
}