using System.Text.Json;
using AvaloniaMySql1125Repository.DB;
using MySqlConnector;

namespace TestProject1;

public class MysqlConnectTests
{
    [SetUp]
    public void Setup()
    {
        MySqlSettings testProps = new MySqlSettings
        {
            Server = "localhost",
            Database = "test",
            Login = "test",
            Password = "test"
        };
        using (var fs  = File.Create("mysql.json"))
            JsonSerializer.Serialize(fs, testProps);
    }

    [Test]
    public void CtorMysqlConnectInitMysqlConnection()
    {
        MySqlConnect testConnect = new();
        var connection = testConnect.Get();
        
        Assert.That(connection, Is.Not.Null);
        Assert.That(connection, Is.InstanceOf<MySqlConnection>());
    }

    [Test]
    public void CtorMysqlConnectInitMysqlConnectionRightArgs()
    {
        MySqlConnect testConnect = new();
        var connection = testConnect.Get();
        Assert.That(connection.Database, Is.EqualTo("test"));
        Assert.That(connection.DataSource, Is.EqualTo("localhost"));
        var keyPairs = connection.ConnectionString.Split(';');
        Assert.That(keyPairs.Contains("Password=test"));
        Assert.That(keyPairs.Contains("User ID=test"));
    }
}