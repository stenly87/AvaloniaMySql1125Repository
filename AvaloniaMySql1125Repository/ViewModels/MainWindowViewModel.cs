using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using AvaloniaMySql1125Repository.DB;
using AvaloniaMySql1125Repository.Models;
using AvaloniaMySql1125Repository.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AvaloniaMySql1125Repository.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty] ObservableCollection<Country> _countries;
    [ObservableProperty] Country _selectedCountry;

    [ObservableProperty] ObservableCollection<Town> _towns;

    public MainWindowViewModel()
    {
        LoadCountries();
    }

    // генерируется метод On[название свойства]Changed(T old, T value)
    partial void OnSelectedCountryChanged(Country oldValue, Country newValue)
    {
        if (newValue == null)
        {
            Towns = new();
            return;
        }

        using (var rep = new TownRepository(
                   new MySqlConnect()))
            Towns = new(rep.GetByCountry(newValue.Id));
    }


    private void LoadCountries()
    {
        // using вызовет Dispose на CountryRepository, а тот - на MySqlConnect
        // данную строку удобнее будет выполнять через DI-контейнер 
        using (var rep = new CountryRepository(
                   new MySqlConnect()))
            Countries = new ObservableCollection<Country>(rep.GetAll());
        // тут инициализация
        // открытие соединения через конструктор
        // выполнение запроса
        // закрытие соединение через dispose
    }

    [RelayCommand]
    public async void AddCountry()
    {
        var country = new Country();
        var win = new EditCountryWindow(country);
        var result = await win.ShowDialog<bool>(GetMain());
        if (result)
            Countries.Add(country);
    }

    Window GetMain()
    {
        Window main = null;
        if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            main = desktop.MainWindow;
        return main;
    }

    [RelayCommand]
    public async void UpdateCountry()
    {
        if (SelectedCountry == null)
            return;
        // берем копию объекта, чтобы не испортить оригинальный объект при отмене редактирования
        var country = new Country { Id = SelectedCountry.Id, Title = SelectedCountry.Title };
        var win = new EditCountryWindow(country);
        var result = await win.ShowDialog<bool>(GetMain());
        if (result)
        {
            Countries.Insert(Countries.IndexOf(SelectedCountry), country);
            Countries.Remove(SelectedCountry);
        }
    }

    [RelayCommand]
    public void RemoveCountry()
    {
        if (SelectedCountry == null)
            return;

        using (var rep = new CountryRepository(
                   new MySqlConnect()))
            if (rep.Delete(SelectedCountry))
                Countries.Remove(SelectedCountry);
    }
}