using Project.BLL.DesignPatterns.GenericRepository.BaseRep;
using Project.BLL.DesignPatterns.GenericRepository.IntRep;
using Project.ENTITIES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Managers
{
    public abstract class BaseManager<T> : IDataAccess<T> where T:BaseEntity
    {
        private BaseRepository<T> repo = new BaseRepository<T>();

        public int Save()
        {
            return repo.Save();
        }
        public virtual int Add(T item)
        {
            return repo.Add(item);
        }

        public void AddRange(List<T> list)
        {
            repo.AddRange(list);
            Save();
        }

        public bool Any(Expression<Func<T, bool>> exp)
        {
            return repo.Any(exp);
        }

        public virtual int Delete(T item)
        {
            return repo.Delete(item);
        }

        public void DeleteRange(List<T> list)
        {
            foreach (T item in list)
            {
                Delete(item);
            }
        }

        public void Destroy(T item)
        {
            repo.Destroy(item);
            Save();
        }

        public void DestroyRange(List<T> list)
        {
            repo.DestroyRange(list);
            Save();
        }

        public T Find(int? id)
        {
            return repo.Find(id);
        }

        public virtual T FirstOrDefault(Expression<Func<T, bool>> exp)
        {
            return repo.FirstOrDefault(exp);
        }

        public virtual List<T> GetActives()
        {
            return repo.GetActives();
        }

        public List<T> GetAll()
        {
            return repo.GetAll();
        }

        public T GetFirstData()
        {
            return repo.GetFirstData();
        }

        public T GetLastData()
        {
            return repo.GetLastData();
        }

        public List<T> GetModifies()
        {
            return repo.GetModifies();
        }

        public List<T> GetPassives()
        {
            return repo.GetPassives();
        }

        public virtual List<T> List(Expression<Func<T, bool>> exp)
        {
            return repo.Where(exp).ToList();
        }

        public object Select(Expression<Func<T, object>> exp)
        {
            return repo.Select(exp);
        }

        public virtual int Update(T item)
        {
            return repo.Update(item);
        }

        public void UpdateRange(List<T> list)
        {
            repo.UpdateRange(list);
            Save();
        }

        public List<T> Where(Expression<Func<T, bool>> exp)
        {
            return repo.Where(exp);
        }

        public IQueryable<T> ListQuaryable()
        {
            return repo.ListQuaryable();
        }
    }
}
