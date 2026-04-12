using System;
namespace FlightTracker.Models;

/// <summary>
/// single flight, matches flights.json
/// </summary>
/// 
public class Flight
{
    public string FlightNumber { get; set; } = string.Empty;
    public string AirlineName { get; set; } = string.Empty;
    public string AirlineCode { get; set; } = string.Empty;
    public string DepartureAirport { get; set; } = string.Empty;
    public string ArrivalAirport { get; set; } = string.Empty;
    public DateTime ScheduledDeparture { get; set; } = default;
    public DateTime ScheduledArrival { get; set; } = default;
    public string AircraftType { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}