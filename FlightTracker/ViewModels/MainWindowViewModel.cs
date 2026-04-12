namespace FlightTracker.ViewModels;

/// <summary>
/// for main window
/// holds navigation between three views that we will create
/// </summary>
public partial class MainWindowViewModel : ViewModelBase
{
    public string Greeting { get; } = "Welcome to Flight Tracker";
}
