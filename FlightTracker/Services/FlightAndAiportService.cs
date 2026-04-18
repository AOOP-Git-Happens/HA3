using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using FlightTracker.Models;

namespace FlightTracker.Services;

public class FlightAndAirportService
{
    public List<Airport> Airports { get; private set; } = new();

    public List<Flight> Flights { get; private set; } = new();

    public FlightAndAirportService()
    {
        LoadFlightsAndAirports();
    }

    public void LoadFlightsAndAirports()
    {
        string filepath = "Assets/flights.json";

        if(!File.Exists(filepath))
        {
            throw new FileNotFoundException($"File not found: {filepath}");
        }
        
        string jsonString = File.ReadAllText(filepath);

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        FlightData? flightData = JsonSerializer.Deserialize<FlightData>(jsonString, options);

        if (flightData != null)
        {
            Airports = flightData.Airports;
            Flights = flightData.Flights;
        }
    }
}