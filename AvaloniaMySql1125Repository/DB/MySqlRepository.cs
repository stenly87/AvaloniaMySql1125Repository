using System;
using System.Collections.Generic;
using MySqlConnector;

namespace AvaloniaMySql1125Repository.DB;

public abstract class MySqlRepository<T> : IDisposable, IRepository<T> where T : class
{
    protected MySqlConnect connector;

    public MySqlRepository(MySqlConnect connector)
    {
        this.connector = connector;
    }

    public abstract T GetById(int id);
    public abstract List<T> GetAll();
    public abstract bool Insert(T entity);
    public abstract bool Update(T entity);
    public abstract bool Delete(T entity);

    public bool OpenConnection()
    {
        try
        {
            connector.Get().Open();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }

    public bool CloseConnection()
    {
        try
        {
            connector.Get().Close();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }

    public bool ExecuteNonQuery(string sql)
    {
        try
        {
            using (var mc = new MySqlCommand(sql, connector.Get()))
                return mc.ExecuteNonQuery() > 0;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }

    public bool ExecuteNonQuery(string sql, MySqlParameter[] parameters)
    {
        try
        {
            using (var mc = new MySqlCommand(sql, connector.Get()))
            {
                mc.Parameters.AddRange(parameters);
                return mc.ExecuteNonQuery() > 0;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }

    public int GetLastID()
    {
        string sql = "select LAST_INSERT_ID()";
        using (var mc = new MySqlCommand(sql, connector.Get()))
            return Convert.ToInt32(mc.ExecuteScalar());
    }

    public void Dispose()
    {
        connector.Dispose();
    }
}