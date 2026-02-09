using System;
using System.Collections.Generic;
using AvaloniaMySql1125Repository.Models;
using MySqlConnector;

namespace AvaloniaMySql1125Repository.DB;

public class CountryRepository : MySqlRepository<Country>, IDisposable
{
    public CountryRepository(MySqlConnect connector) : base(connector)
    {
        OpenConnection();
    }

    public override Country GetById(int id)
    {
        Country result = new Country();
        string sql = "select `Id`, `Title` from `Country` where `Id` = " + id;
        using (var mc = new MySqlCommand(sql, connector.Get()))
        using (var reader = mc.ExecuteReader())
        {
            if (reader.Read())
            {
                result.Id = reader.GetInt32("Id");
                result.Title = reader.GetString("Title");
            }
        }

        return result;
    }

    public override List<Country> GetAll()
    {
        List<Country> result = new();
        string sql = "select `Id`, `Title` from `Country`";
        using (var mc = new MySqlCommand(sql, connector.Get()))
        using (var reader = mc.ExecuteReader())
        {
            while (reader.Read())
            {
                var row = new Country
                {
                    Id = reader.GetInt32("Id"),
                    Title = reader.GetString("Title")
                };
                result.Add(row);
            }
        }

        return result;
    }

    public override bool Insert(Country entity)
    {
        string sql = "insert into `Country` (`Id`, `Title`) values (0, @Title)";
        var parameters = new MySqlParameter[] { new MySqlParameter("@Title", entity.Title) };
        return ExecuteNonQuery(sql, parameters);
    }

    public override bool Update(Country entity)
    {
        string sql = "update  `Country` set `Title` = @Title where `Id` = " + entity.Id;
        var parameters = new MySqlParameter[] { new MySqlParameter("@Title", entity.Title) };
        return ExecuteNonQuery(sql, parameters);
    }

    public override bool Delete(Country entity)
    {
        string sql = "delete from `Country` where `Id` = " + entity.Id;
        return ExecuteNonQuery(sql);
    }

    public new void Dispose()
    {
        CloseConnection();
        base.Dispose();
    }
}