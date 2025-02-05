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
    private string _uri;
    [ObservableProperty]
    private int _id;
    [ObservableProperty]
    private string _serverName;


    [RelayCommand]
    private async Task ChangeStrategy(string id)
    {
        Id = int.Parse(id);
        OnPropertyChanged(nameof(Id));
        await SendStrategyChange();
    }

    [RelayCommand]
    private void ChangeServer(string servername)
    {
        ServerName = servername;
        SetUri(servername);
        OnPropertyChanged(nameof(ServerName));
    }
    
    private async Task SendStrategyChange()
    {
        await _httpClient.PostAsJsonAsync($"{_uri}/setmove/{Id}", Id);
    }
    private void SetUri(string servername)
    {
        switch (servername)
        {
            case "Server 1":
                _uri = "https://badgerclanloganbot1-hqh7htb3gkf2gbes.westus-01.azurewebsites.net";
                break;
            case "Server 2":
                _uri = "";
                break;
            default:
                _uri = "http://localhost:5217";
                break;
        }
    }
    public MainViewModel(HttpClient httpClient)
    {
        Id = 2;
        ServerName = "Local";
        _uri = "http://localhost:5217";
        _httpClient = httpClient;
    }
}
