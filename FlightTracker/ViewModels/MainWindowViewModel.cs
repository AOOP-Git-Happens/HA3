using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using FlightTracker.Services;

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
        var service = new FlightAndAirportService();
        
        Tabs.Add(new RouteMapViewModel(service));
        Tabs.Add(new InfoFlightViewModel(service));
        Tabs.Add(new AnalyticsViewModel(service));

        // Set the Map as the default tab when the app opens
        SelectedTab = Tabs[0]; 
    }
}

