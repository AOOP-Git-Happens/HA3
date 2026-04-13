using CommunityToolkit.Mvvm.ComponentModel;

namespace FlightTracker.ViewModels;

/// <summary>
/// base class for all ViewModels
/// inherit from ObservableObject to notify UI about changes
/// </summary>
public abstract class ViewModelBase : ObservableObject
{
    [ObservableProperty]
    private string _header = string.Empty;
}
