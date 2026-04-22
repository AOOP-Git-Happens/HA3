using System;
using System.IO;
using System.Text.Json;

namespace FlightTracker.Services;

public class PreferenceService
{
    private readonly string _filePath;

    public PreferenceService()
    {
        _filePath = "settings.json";
    }

    public void Save(string searchText, string? airportCode, string? flightNumber)
    {
        try
        {
            var data = new UserPrefsData 
            { 
                SearchText = searchText, 
                AirportCode = airportCode,
                FlightNumber = flightNumber
            };
            
            string json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, json);
        }
        
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error saving : {ex.Message}");
        }
    }

    public (string SearchText, string? AirportCode, string? flightNumber) Load()
    {   
        if (!File.Exists(_filePath)) 
        {
            return ("", null, null);
        }

        try
        {
            string json = File.ReadAllText(_filePath);
            var data = JsonSerializer.Deserialize<UserPrefsData>(json);
            
            return (data?.SearchText ?? "", data?.AirportCode, data?.FlightNumber);
        }

        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error loading preferences: {ex.Message}");
            return ("", null, null);
        }
    }

    private class UserPrefsData
    {
        public string SearchText { get; set; } = "";
        public string? AirportCode { get; set; }
        public string? FlightNumber { get; set; }
    }
}