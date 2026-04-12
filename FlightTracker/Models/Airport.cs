namespace FlightTracker.Models;

/// <summary>
/// Represents an airport from the JSON data.
/// Used in map visualization and flight relations.
/// </summary>
public class Airport
{
    public string IataCode { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}