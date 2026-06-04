using System.Text.Json;
using AvaloniaMySql1125Repository.DB;
using AvaloniaMySql1125Repository.Models;
using AvaloniaMySql1125Repository.ViewModels;
using MySqlConnector;

namespace TestProject1.ViewModels;

public class EditCountryVMTests
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
    public void VMSetSelectedCountryAfterCtor()
    {
        var country = new Country();
        
        EditCountryWindowViewModel vm = new(country, s => { });
        
        Assert.That(vm.SelectedCountry, Is.EqualTo(country));
    }
    
    [Test]
    public void VMSaveCountrySuccess()
    {
        var country = new Country{ Title = "РФ"};
        
        EditCountryWindowViewModel vm = new(country, s => { });
        vm.Save();

        CountryRepository countryRepository = new(testConnect);
        var countries = countryRepository.GetAll();
        Assert.That(countries.Count, Is.EqualTo(1));
        Assert.That(countries.Any(s=>s.Title.Equals(country.Title)), Is.True);
    }
    
    
}