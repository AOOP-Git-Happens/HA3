using System;
using CommunityToolkit.Mvvm.ComponentModel;
using FlightTracker.Models;
using FlightTracker.Services;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.Input;

namespace FlightTracker.ViewModels;

/// <summary>
/// ViewModel 2. Shows airport info details, filters by status,
/// export of selected data.
/// </summary>

public partial class InfoFlightViewModel : ViewModelBase
{
    private readonly FlightAndAirportService _flightAndAirportService;

    [ObservableProperty]
    private string searchText = "";
    partial void OnSearchTextChanged(string value) //generated partial method when property changes
    {
        FilterAirports();
    }
    
    [ObservableProperty]
    private Flight? selectedFlight;

    [ObservableProperty]
    private Airport? selectedAirport;
    partial void OnSelectedAirportChanged(Airport? value)
    {
        FilterFlights();
    }

    public ObservableCollection<Flight> Flights { get; } = new();
    public ObservableCollection<Airport> Airports { get; } = new();
    public ObservableCollection<Airport> FilteredAirports { get; } = new();
    public ObservableCollection<Flight> FilteredFlights { get; } = new();

    public InfoFlightViewModel(FlightAndAirportService flightAndAirportService)
    {
        Header = "Flights"; //tab name

        _flightAndAirportService = flightAndAirportService;

        foreach (var flight in flightAndAirportService.Flights)
        {
            Flights.Add(flight);
        }
        
        foreach (var airport in flightAndAirportService.Airports)
        {
            Airports.Add(airport);
        }

        FilterAirports();
        FilterFlights();
    }

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

    private void FilterFlights()
{
    FilteredFlights.Clear();

    var query = SelectedAirport == null 
        ? Flights 
        : Flights.Where(f => f.DepartureAirport == SelectedAirport.IataCode);

    foreach (var flight in query)
    {
        FilteredFlights.Add(flight);
    }
}

    [RelayCommand]
    private void ClearSelection()
    {
        SelectedAirport = null;
        SearchText = "";
        FilterAirports();
    }

    [RelayCommand]
    private void FilterFlightsAll()
    {
        FilterFlights();
    }

    [RelayCommand]
    private void FilterFlightsLanded()
    {
        FilteredFlights.Clear();

        var query = SelectedAirport == null 
            ? Flights 
            : Flights.Where(f => f.DepartureAirport == SelectedAirport.IataCode &&
                                f.Status == "Landed");

        foreach (var flight in query)
        {
            FilteredFlights.Add(flight);
        }
    }

    [RelayCommand]
    private void FilterFlightsScheduled()
    {
        FilteredFlights.Clear();

        var query = SelectedAirport == null 
            ? Flights 
            : Flights.Where(f => f.DepartureAirport == SelectedAirport.IataCode &&
                                f.Status == "Scheduled");

        foreach (var flight in query)
        {
            FilteredFlights.Add(flight);
        }
    }
}