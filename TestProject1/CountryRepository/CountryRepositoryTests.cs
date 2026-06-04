using System.Text.Json;
using AvaloniaMySql1125Repository.DB;
using AvaloniaMySql1125Repository.Models;
using MySqlConnector;

namespace TestProject1;

public class CountryRepositoryTests
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
        using (var mc = new MySqlCommand("delete from `Towns`", testConnect.Get()))
        {
            mc.ExecuteNonQuery();
            mc.CommandText = "delete from `Country`";
            mc.ExecuteNonQuery();
        }
        testConnect.Get().Close();
    }
    // интеграционный тест (поскольку проверяется несколько методов/объектов)
    [Test]
    public void CountryRepositoryInsertSuccess()
    {
        CountryRepository countryRep = new CountryRepository(testConnect);
        Country test = new Country
        {
            Title = "РФ"
        };
        
        bool result = countryRep.Insert(test);

        Assert.That(result, Is.True);
    }
}