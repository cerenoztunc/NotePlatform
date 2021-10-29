using Project.ENTITIES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.DesignPatterns.GenericRepository.IntRep
{
    public interface IDataAccess<T> where T:BaseEntity
    {
        //List Commands
        List<T> GetAll();
        List<T> GetActives();
        List<T> GetPassives();
        List<T> GetModifies();

        //Modify Commands
        int Add(T item);
        void AddRange(List<T> list);
        int Delete(T item);
        void DeleteRange(List<T> list);
        int Update(T item);
        void UpdateRange(List<T> list);
        int Destroy(T item);
        void DestroyRange(List<T> list);
       

        //Linq Commands
        List<T> Where(Expression<Func<T, bool>> exp);
        bool Any(Expression<Func<T, bool>> exp);
        T FirstOrDefault(Expression<Func<T, bool>> exp);
        object Select(Expression<Func<T, object>> exp);
        List<T> List(Expression<Func<T, bool>> exp);
        IQueryable<T> ListQuaryable();

        //FindCommands
        T Find(int? id);
        T GetLastData();
        T GetFirstData();
    }
}
