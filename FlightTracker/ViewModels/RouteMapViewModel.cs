using System;
using CommunityToolkit.Mvvm.ComponentModel;

using FlightTracker.Models;
using FlightTracker.Services;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Linq;
using System.Collections.Generic;
namespace FlightTracker.ViewModels;

/// <summary>
/// ViewModel 1. Contains airport search, map integration,
/// selected airports, route visualisation, clear/reset
/// </summary>

public partial class RouteMapViewModel : ViewModelBase
{
    private readonly FlightAndAirportService _flightAndAirportService;

    //search text + automatic filtering-- what should be improved for this task 
    [ObservableProperty]
    private string searchText = "";
    partial void OnSearchTextChanged(string value) //generated partial method when property changes
    {
        FilterAirports();
    }

    //selected item, update later routes, markers, map lines
    [ObservableProperty]
    private Airport? selectedAirport;
    partial void OnSelectedAirportChanged(Airport? value)
    {
        UpdateRoutes();
    }

    //all airports
    public ObservableCollection<Airport> Airports { get; } = new();
    //filter for UI
    public ObservableCollection<Airport> FilteredAirports { get; } = new();
    //route result
    public ObservableCollection<Airport> DestinationAirports { get; } = new();


    public RouteMapViewModel(FlightAndAirportService flightAndAirportService)
    {
        Header = "Map"; //tab name

        _flightAndAirportService = flightAndAirportService;

        foreach (var airport in flightAndAirportService.Airports)
        {
            Airports.Add(airport);
        }

        FilterAirports();
    }

    //filtering collection
    private void FilterAirports()
    {
        FilteredAirports.Clear();

        var filtered = string.IsNullOrWhiteSpace(SearchText) ? Airports : new ObservableCollection<Airport>(
            Airports.Where(a =>
                a.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                a.City.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                a.IataCode.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                a.Country.Contains(SearchText, StringComparison.OrdinalIgnoreCase)));

        foreach (var airport in filtered)
        {
            FilteredAirports.Add(airport);
        }
    }

    [RelayCommand]
    private void ClearSelection()
    {
        SelectedAirport = null;
        SearchText = "";
        FilterAirports();
    }

    ///route data, "which airports can I fly to from the selected airport?"
    //takes the selected airport's IATA code, finds all matching flights,
    //extracts unique arrival airport codes, and converts them into Airport objects
    private void UpdateRoutes()
    {
        // Remove old route results before building new ones
        DestinationAirports.Clear();

        // Stop immediately if the user has not selected an airport
        if (SelectedAirport == null)
            return;

        // Get all unique destination airport codes for flights
        // leaving from the selected airport
        var arrivalCodes = _flightAndAirportService.Flights
            .Where(f => f.DepartureAirport == SelectedAirport.IataCode)
            .Select(f => f.ArrivalAirport)
            .Distinct();

        // Find all Airport objects whose IATA code is in the destination list
        var destinationAirports = _flightAndAirportService.Airports
            .Where(a => arrivalCodes.Contains(a.IataCode));

        // Add the results to the observable collection so the UI updates
        foreach (var airport in destinationAirports)
        {
            DestinationAirports.Add(airport);
        }
    }
}