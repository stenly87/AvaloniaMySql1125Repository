using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using AvaloniaMySql1125Repository.Models;
using AvaloniaMySql1125Repository.ViewModels;

namespace AvaloniaMySql1125Repository.Views;

public partial class EditCountryWindow : Window
{
    public EditCountryWindow(Country country)
    {
        InitializeComponent();
        DataContext = new EditCountryWindowViewModel(country, Close);
    }
}