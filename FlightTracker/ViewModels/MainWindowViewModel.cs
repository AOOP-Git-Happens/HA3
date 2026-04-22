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
        var service = new FlightAndAirportService();
        var preferenceService = new PreferenceService(); 
        var exportService = new FlightExportService();
        
        Tabs.Add(new RouteMapViewModel(service));
        Tabs.Add(new InfoFlightViewModel(service, preferenceService, exportService)); 
        Tabs.Add(new AnalyticsViewModel(service));
        
        SelectedTab = Tabs[0]; 
    }
}