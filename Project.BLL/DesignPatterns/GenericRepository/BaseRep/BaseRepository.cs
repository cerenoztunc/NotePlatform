using Project.DAL.Context;
using Project.BLL.DesignPatterns.GenericRepository.IntRep;
using Project.BLL.DesignPatterns.SingletonPattern;
using Project.ENTITIES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Project.COMMON;

namespace Project.BLL.DesignPatterns.GenericRepository.BaseRep
{
    public class BaseRepository<T> : IDataAccess<T> where T : BaseEntity
    {
        protected MyContext _db;
        public BaseRepository()
        {
            _db = DBTool.DBInstance;
        }
        public int Save()
        {
           return _db.SaveChanges();
        }

        public int Add(T item)
        {
            _db.Set<T>().Add(item);
            if(item is BaseEntity)
            {
                BaseEntity be = item as BaseEntity;
                
                be.ModifiedUserName = App.Common.GetCurrentUsername();
            }
            return _db.SaveChanges();
        }

        public void AddRange(List<T> list)
        {
            _db.Set<T>().AddRange(list);
            Save();
        }

        public bool Any(Expression<Func<T, bool>> exp)
        {
            return _db.Set<T>().Any(exp);
        }

        public int Delete(T item)
        {
            item.Status = ENTITIES.Enums.DataStatus.Deleted;
            item.DeletedDate = DateTime.Now;
            var sc = _db.SaveChanges();
            return sc;
        }

        public void DeleteRange(List<T> list)
        {
            foreach (T item in list)
            {
                Delete(item);
            }
        }

        public int Destroy(T item)
        {
            _db.Set<T>().Remove(item);
            return Save();
        }

        public void DestroyRange(List<T> list)
        {
            _db.Set<T>().RemoveRange(list);
            Save();
        }

        public T Find(int? id)
        {
            return _db.Set<T>().Find(id.Value);
        }

        public T FirstOrDefault(Expression<Func<T, bool>> exp)
        {
            return _db.Set<T>().FirstOrDefault(exp);
        }

        public List<T> GetActives()
        {
            return Where(x => x.Status != ENTITIES.Enums.DataStatus.Deleted);
        }

        public List<T> GetAll()
        {
            return _db.Set<T>().ToList();
        }

        public T GetFirstData()
        {
            return _db.Set<T>().OrderBy(x => x.CreatedDate).FirstOrDefault();
        }

        public T GetLastData()
        {
            return _db.Set<T>().OrderByDescending(x => x.CreatedDate).FirstOrDefault();
        }

        public List<T> GetModifies()
        {
            return Where(x => x.Status == ENTITIES.Enums.DataStatus.Updated);
        }

        public List<T> GetPassives()
        {
            return Where(x => x.Status == ENTITIES.Enums.DataStatus.Deleted);
        }

        public object Select(Expression<Func<T, object>> exp)
        {
            return _db.Set<T>().Select(exp).ToList();
        }

        public int Update(T item)
        {
            item.Status = ENTITIES.Enums.DataStatus.Updated;
            item.ModifiedDate = DateTime.Now;
            T toBeUpdated = Find(item.ID);
            _db.Entry(toBeUpdated).CurrentValues.SetValues(item);
            if (item is BaseEntity)
            {
                BaseEntity be = item as BaseEntity;
                
                be.ModifiedUserName = App.Common.GetCurrentUsername();
            }
            return _db.SaveChanges();
        }

        public void UpdateRange(List<T> list)
        {
            foreach (T item in list)
            {
                Update(item);
            }
        }

        public List<T> Where(Expression<Func<T, bool>> exp)
        {
            return _db.Set<T>().Where(exp).ToList();
        }

        public List<T> List(Expression<Func<T, bool>> exp)
        {
            return _db.Set<T>().Where(exp).ToList();
        }
        
        public IQueryable<T> ListQuaryable()
        {
            return _db.Set<T>().AsQueryable();
        }
    }
}
