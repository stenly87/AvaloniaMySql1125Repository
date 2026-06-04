using System.Text.Json;
using AvaloniaMySql1125Repository.DB;
using AvaloniaMySql1125Repository.Models;
using MySqlConnector;

namespace TestProject1;

public class UpdateTests
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
            mc.CommandText = "insert into `Towns` values (1, 'Влад', 1)";
            mc.ExecuteNonQuery();
        }
        testConnect.Get().Close();
    }

    [Test]
    public void UpdateTownSuccess()
    {
        Town town = new Town { Id = 1, Title = "Уссурийск", CountryId = 1};
        
        TownRepository townRepo = new TownRepository(testConnect);
        
        var resultSql = townRepo.Update(town);
        Assert.That(resultSql, Is.True);

        var towns = townRepo.GetAll();
        var result = towns.FirstOrDefault(s => s.Id == town.Id);
        
        Assert.That(result.Title, Is.EqualTo(town.Title));
    }
}