using System;
using System.Collections.Generic;
using AvaloniaMySql1125Repository.Models;
using MySqlConnector;

namespace AvaloniaMySql1125Repository.DB;

public class TownRepository: MySqlRepository<Town>, IDisposable
{
    public TownRepository(MySqlConnect connect) : base(connect)
    {
        OpenConnection();
    }

    public override Town GetById(int id)
    {
        Town result = new Town();
        string sql = "select t.Id, t.Title, t.IdCountry, c.Title as 'country' from Towns t join Country c on t.IdCountry  = c.Id where t.`Id` = " + id;
        using (var mc = new MySqlCommand(sql, connector.Get())) 
        using (var reader = mc.ExecuteReader())
        {
            if (reader.Read())
            {
                result.Id = reader.GetInt32("Id");
                result.Title = reader.GetString("Title");
                result.Country = new Country();
                result.Country.Id = reader.GetInt32("IdCountry");
                result.Country.Title = reader.GetString("country");
            }
        }

        return result;
    }

    List<Town> GetBySql(string sql)
    {
        List<Town> result = new ();
        using (var mc = new MySqlCommand(sql, connector.Get())) 
        using (var reader = mc.ExecuteReader())
        {
            if (reader.Read())
            {
                var town = new Town();
                town.Id = reader.GetInt32("Id");
                town.Title = reader.GetString("Title");
                town.Country = new Country();
                town.Country.Id = reader.GetInt32("IdCountry");
                town.Country.Title = reader.GetString("country");
                result.Add(town);
            }
        }

        return result;
    }

    public override List<Town> GetAll()
    {
        string sql = "select t.Id, t.Title, t.IdCountry, c.Title as 'country' from Towns t join Country c on t.IdCountry  = c.Id";
        return GetBySql(sql);
    }
    
    public List<Town> GetByCountry(int countryId)
    {
        string sql = "select t.Id, t.Title, t.IdCountry, c.Title as 'country' from Towns t join Country c on t.IdCountry  = c.Id where t.`IdCountry` = " + countryId;
        return GetBySql(sql);
    }

    public override bool Insert(Town entity)
    {
        string sql = "insert into `Towns` (`Id`, `Title`, `IdCountry`) values (0, @Title, @IdCountry)";
        var parameters = new MySqlParameter[ ]
        {
            new MySqlParameter("@Title", entity.Title),
            new MySqlParameter("@IdCountry", entity.CountryId)
        };
        return ExecuteNonQuery(sql, parameters);
    }

    public override bool Update(Town entity)
    {
        string sql = "update `Towns` set `Title` = @Title, `IdCountry` = @IdCountry where `Id` = " + entity.Id;
        var parameters = new MySqlParameter[ ]
        {
            new MySqlParameter("@Title", entity.Title),
            new MySqlParameter("@IdCountry", entity.CountryId)
        };
        return ExecuteNonQuery(sql, parameters);
    }

    public override bool Delete(Town entity)
    {
        string sql = "delete from `Towns` where `Id` = " + entity.Id;
        return ExecuteNonQuery(sql);
    }
    
    public new void Dispose()
    {
        CloseConnection();
        base.Dispose();
    }
}