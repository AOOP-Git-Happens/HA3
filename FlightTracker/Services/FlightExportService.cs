using System;
using System.IO;
using System.Text.Json;
using CommunityToolkit.Mvvm.Input;

namespace FlightTracker.Services;

public class FlightExportService
{
    private readonly string _filePath;

    public FlightExportService()
    {
        _filePath = "flightExport.json";
    }

    public void ExportFlight(string? flightNumber, string? flightName, string? flightDepartureAirport, string? flightArrivalAirport, string? flightStatus)
    {
        try
        {
            var data = new FlightData
            { 
                FlightNumber = flightNumber,
                AirlineName = flightName, 
                Departure = flightDepartureAirport,
                Arrival = flightArrivalAirport,
                Status = flightStatus,
            };
            
            string json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, json);
        }
        
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error saving flight data : {ex.Message}");
        }
    }

    private class FlightData
    {
        public string? FlightNumber { get; set; }
        public string? AirlineName { get; set; }
        public string? Departure { get; set; }
        public string? Arrival { get; set; }
        public string? Status { get; set; }
    }
}