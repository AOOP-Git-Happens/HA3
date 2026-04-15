using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;

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
}