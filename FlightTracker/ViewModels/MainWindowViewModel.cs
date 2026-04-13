using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace FlightTracker.ViewModels;

/// <summary>
/// for main window
/// holds navigation between three views that we will create
/// </summary>
public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private ViewModelBase _selectedTab;

    public ObservableCollection<ViewModelBase> Tabs { get; } = new();


    public string Greeting { get; } = "Welcome to Flight Tracker";

    public MainWindowViewModel()
    {
        // Add the three pages to our observable list
        Tabs.Add(new RouteMapViewModel());
        Tabs.Add(new InfoFlightViewModel());
        Tabs.Add(new AnalyticsViewModel());

        // Set the Map as the default tab when the app opens
        SelectedTab = Tabs[0]; 
    }
}

