using CommunityToolkit.Mvvm.ComponentModel;
using FlightTracker.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace FlightTracker.ViewModels;

/// <summary>
/// ViewModel 2. Shows airport info details, filters by status,
/// export of selected data.
/// </summary>

public partial class InfoFlightViewModel : ViewModelBase
{
    
    [ObservableProperty]
    private Flight? selectedFlight;

    public ObservableCollection<Flight> Flights { get; } = new();

    public InfoFlightViewModel()
    {
        Header = "Flights"; //tab name

        //Samples
        Flights.Add(new Flight{
            FlightNumber	 = "SK0451",
            AirlineName	     = "Scandinavian Airlines",
            AirlineCode	     = "SK",
            DepartureAirport = "CPH",
            ArrivalAirport	 = "AMS",
            AircraftType	 = "Airbus A320",
            Status           = "Landed"});
        Flights.Add(new Flight{
            FlightNumber	 = "SK0474",
            AirlineName	     = "Ryanair",
            AirlineCode	     = "SK",
            DepartureAirport = "AMS",
            ArrivalAirport	 = "CPH",
            AircraftType	 = "Airbus A321",
            Status           = "Scheduled"});
    }
}