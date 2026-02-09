using System;
using AvaloniaMySql1125Repository.DB;
using AvaloniaMySql1125Repository.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AvaloniaMySql1125Repository.ViewModels;

public partial class EditCountryWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    Country _selectedCountry;

    private Action<object?> close;
    public EditCountryWindowViewModel(Country country, Action<object?> close)
    {
        SelectedCountry = country;
        this.close = close; 
    }

    [RelayCommand]
    public void Save()
    {
        using (var rep = new CountryRepository(
                   new MySqlConnect()))
            if (SelectedCountry.Id == 0)
            {
                rep.Insert(SelectedCountry);
                SelectedCountry.Id = rep.GetLastID();
            }
            else
                rep.Update(SelectedCountry);
        close(true);
    }
}