using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using MySqlConnector;

namespace AvaloniaMySql1125Repository.DB;

public class MySqlConnect : IDisposable
{
    private MySqlConnection _connection;
    private string fileSettings = "mysql.json";

    public MySqlConnect()
    {
        MySqlSettings settings;
        try
        {
            string settingsContent = File.ReadAllText(fileSettings);
            settings = JsonSerializer.Deserialize<MySqlSettings>(settingsContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            _connection = new();
            return;
        }

        MySqlConnectionStringBuilder sb = new();
        sb.Server = settings.Server;
        sb.Database = settings.Database;
        sb.UserID = settings.Login;
        sb.Password = settings.Password;
        _connection = new MySqlConnection(sb.ToString());
    }

    public MySqlConnection Get() => _connection;

    public void Dispose()
    {
        _connection.Dispose();
    }
}