using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using System;

namespace FlightTracker.ViewModels;

/// <summary>
/// ViewModel 3. LINQ analytics, charts, export.
/// </summary>

public partial class AnalyticsViewModel : ViewModelBase
{
    public AnalyticsViewModel()
    {
        Header = "Analytics"; // tab name
    }

    [RelayCommand]
    private void Export()
    {
        // To be implemented later
    }

    [ObservableProperty]
    private DateTimeOffset? _SelectedDate;
}