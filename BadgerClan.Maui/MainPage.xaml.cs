using BadgerClan.Maui.ViewModels;
namespace BadgerClan.Maui;

public partial class MainPage : ContentPage
{

    public MainPage(MainViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
