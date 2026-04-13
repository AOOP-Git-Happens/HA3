namespace FlightTracker.ViewModels;

/// <summary>
/// ViewModel 2. Shows airport info details, filters by status,
/// export of selected data.
/// </summary>

public partial class InfoFlightViewModel : ViewModelBase
{
     public InfoFlightViewModel()
    {
        Header = "Flights"; //tab name
    }
}