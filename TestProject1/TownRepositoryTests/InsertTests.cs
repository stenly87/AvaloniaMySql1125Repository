using System.Text.Json;
using AvaloniaMySql1125Repository.DB;
using AvaloniaMySql1125Repository.Models;
using MySqlConnector;

namespace TestProject1;

public class InsertTests
{
    private MySqlConnect testConnect;
    [SetUp]
    public void Setup()
    {
        MySqlSettings test= new MySqlSettings
        {
            Server = "192.168.200.13",
            Database = "fake_1125_2026_towns",
            Login = "student",
            Password = "student"
        };
        using (var fs  = File.Create("mysql.json"))
            JsonSerializer.Serialize(fs, test);
        testConnect = new();
        // либо бд должна быть в докере
        // либо тут инициализация таблиц
        testConnect.Get().Open();
        using (var mc = new MySqlCommand("delete from `Country`", testConnect.Get()))
        {
            mc.ExecuteNonQuery();
            mc.CommandText = "delete from `Towns`";
            mc.ExecuteNonQuery();
            mc.CommandText = "insert into `Country` values (1, 'РФ')";
            mc.ExecuteNonQuery();
        }
        testConnect.Get().Close();
    }

    [Test]
    public void TownRepositoryInsertDataRight()
    {
        TownRepository repo = new(testConnect);
        Town town = new Town{ Title = "Владивосток", CountryId = 1 };
        repo.OpenConnection();
        repo.Insert(town);

        var towns = repo.GetAll();
        var result = towns.FirstOrDefault(s => s.Title.Equals(town.Title));
        repo.CloseConnection();
        Assert.That(result, Is.Not.Null);   
        Assert.That(result.CountryId, Is.EqualTo(town.CountryId));
    }

    [Test]
    public void TownRepositoryThrowExceptionOnValidateArg()
    {
        TownRepository fakeRepo = new (testConnect);
        Town town = new();
        Assert.Catch(typeof(Exception),
            () => fakeRepo.Insert(town), 
            "Validate town fails");
    }
    
}