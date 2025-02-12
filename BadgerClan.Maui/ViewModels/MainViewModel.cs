using BadgerClan.Logic.Services;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Grpc.Net.Client;
using ProtoBuf.Grpc.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    [ObservableProperty]
    private ObservableCollection<Server> _servers;

    [ObservableProperty]
    private string _grpcUri;

    [ObservableProperty]
    private StrategySwapResponse _response;

    [RelayCommand]
    private void AddServer()
    {
        Servers.Add(new Server { Name = GrpcUri, Uri = GrpcUri, IsChecked = false, IsGrcp = true });
        OnPropertyChanged(nameof(Servers));
    }

    [RelayCommand]
    private async Task ChangeStrategy(string id)
    {
        Id = int.Parse(id);
        foreach (var server in Servers.Where(i => i.IsChecked))
        {
            if (server.IsGrcp)
            {
               await StrategySwapGrpc(server);
            }
            else
            {
                await SendStrategyChange(server.Uri);
            }
        }
        OnPropertyChanged(nameof(Id));
    }
    
    private async Task SendStrategyChange(string uri)
    {
        await _httpClient.PostAsJsonAsync($"{uri}/setmove/{Id}", Id);
    }

    private async Task StrategySwapGrpc(Server server)
    {
        GrpcClientFactory.AllowUnencryptedHttp2 = true;
        using var channel = GrpcChannel.ForAddress(server.Uri);
        var swapClient = channel.CreateGrpcService<IStrategySwapService>();
        var Response = await swapClient.StrategySwap(new StrategySwapRequest
        {
            StrategyId = Id
        });
    }
    public MainViewModel(HttpClient httpClient)
    {
        Id = 2;
        _httpClient = httpClient;
        Servers = new()
        {
            new Server() {Name = "Local", Uri =  "http://localhost:5217", IsChecked = true},
            new Server() {Name = "Server 1", Uri =  "https://badgerclanloganbot1-hqh7htb3gkf2gbes.westus-01.azurewebsites.net", IsChecked = false},
            new Server() {Name = "Server 2", Uri =  "https://badgerclanloganbot2-g3bfaqgge7fqbtfp.canadacentral-01.azurewebsites.net", IsChecked = false}
            //new Server() {Name = "Grpc", Uri = "http://localhost:7232", IsChecked = false}
        };
  

    }
}

public class Server
{
    public string Name { get; set; }
    public string Uri { get; set;  }
    public bool IsChecked { get; set; }
    public bool IsGrcp { get; set; } = false;
}
