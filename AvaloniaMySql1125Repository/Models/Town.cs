namespace AvaloniaMySql1125Repository.Models;

public class Town : BaseTable
{
    public string Title { get; set; }
    
    public int CountryId { get; set; }
    
    public Country Country { get; set; }
}