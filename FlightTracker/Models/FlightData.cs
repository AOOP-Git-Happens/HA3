using System.Collections.Generic;

namespace FlightTracker.Models;

/// <summary>
/// Root object of the JSON file.
/// Contains all airports and flights.
/// </summary>
public class FlightData
{
    public List<Airport> Airports { get; set; } = new();

    public List<Flight> Flights { get; set; } = new();
}