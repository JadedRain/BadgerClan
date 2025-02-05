using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BadgerClan.Maui.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private HttpClient _httpClient;
    [ObservableProperty]
    private int _id;

    [RelayCommand]
    private async Task ChangeStrategy(string id)
    {
        Id = int.Parse(id);
        OnPropertyChanged(nameof(Id));
        await SendStrategyChange();
    }
    
    private async Task SendStrategyChange()
    {
        await _httpClient.PostAsJsonAsync($"https://badgerclanloganbot1-hqh7htb3gkf2gbes.westus-01.azurewebsites.net/setmove/{Id}", Id);
    }
    public MainViewModel(HttpClient httpClient)
    {
        Id = 2;
        _httpClient = httpClient;
    }
}
