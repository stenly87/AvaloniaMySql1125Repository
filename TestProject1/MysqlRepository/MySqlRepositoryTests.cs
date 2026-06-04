using System.Text.Json;
using AvaloniaMySql1125Repository.DB;
using AvaloniaMySql1125Repository.Models;
using MySqlConnector;

namespace TestProject1;

public class MySqlRepositoryTests
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

    [Test]
    public void MysqlRepositoryOpenConnectionSuccess()
    {
        MysqlFakeRepository fake = new MysqlFakeRepository(testConnect);
        try
        {
            fake.OpenConnection();
        }
        catch (Exception e)
        {
            Assert.Fail(e.Message);
        }
        finally
        {
            fake.CloseConnection();
            Assert.Pass();
        }
    }

    
    
}