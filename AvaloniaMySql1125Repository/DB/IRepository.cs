using System.Collections.Generic;

namespace AvaloniaMySql1125Repository.DB;

public interface IRepository<T> where T : class
{
    T GetById(int id);
    List<T> GetAll();
    bool Insert(T entity);
    bool Update(T entity);
    bool Delete(T entity);
    
    bool OpenConnection();
    bool CloseConnection();
}