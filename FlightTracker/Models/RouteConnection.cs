using System.Collections.Generic;

namespace FlightTracker.Models;

/// <summary>
/// connection between two airports
/// useful for route drawing and filtering
/// </summary>
public class RouteConnection
{
    public string DepartureAirport { get; set; } = string.Empty;
    public string ArrivalAirport { get; set; } = string.Empty;
}