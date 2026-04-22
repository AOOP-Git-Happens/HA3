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
    private readonly PreferenceService _preferenceService;
    private readonly FlightExportService _flightExportService;

    [ObservableProperty]
    private string searchText = "";
    partial void OnSearchTextChanged(string value) //generated partial method when property changes
    {
        FilterAirports();
        _preferenceService.Save(SearchText, SelectedAirport?.IataCode, SelectedFlight?.FlightNumber);
    }
    
    [ObservableProperty]
    private Flight? selectedFlight;
    partial void OnSelectedFlightChanged(Flight? value)
    {
        _preferenceService.Save(SearchText, SelectedAirport?.IataCode, value?.FlightNumber);
    }

    [ObservableProperty]
    private Airport? selectedAirport;
    partial void OnSelectedAirportChanged(Airport? value)
    {
        FilterFlights();
        _preferenceService.Save(SearchText, SelectedAirport?.IataCode, SelectedFlight?.FlightNumber);
    }

    public ObservableCollection<Flight> Flights { get; } = new();
    public ObservableCollection<Airport> Airports { get; } = new();
    public ObservableCollection<Airport> FilteredAirports { get; } = new();
    public ObservableCollection<Flight> FilteredFlights { get; } = new();

    public InfoFlightViewModel(FlightAndAirportService flightAndAirportService, PreferenceService preferenceService, FlightExportService flightExportService)
    {
        Header = "Flights"; //tab name

        _flightAndAirportService = flightAndAirportService;
        _preferenceService = preferenceService;
        _flightExportService = flightExportService;

        foreach (var flight in flightAndAirportService.Flights)
        {
            Flights.Add(flight);
        }
        
        foreach (var airport in flightAndAirportService.Airports)
        {
            Airports.Add(airport);
        }

        var (savedSearch, savedCode, savedFlightNumber) = _preferenceService.Load();
        
        searchText = savedSearch;
        selectedAirport = Airports.FirstOrDefault(a => a.IataCode == savedCode);

        FilterAirports();
        FilterFlights();

        SelectedFlight = FilteredFlights.FirstOrDefault(f => f.FlightNumber == savedFlightNumber);
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
    private void ExportButtonPress()
    {
        if (SelectedFlight != null)
        {
            _flightExportService.ExportFlight(
                SelectedFlight.FlightNumber,
                SelectedFlight.AirlineName,
                SelectedFlight.DepartureAirport,
                SelectedFlight.ArrivalAirport,
                SelectedFlight.Status
            );
        }
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

        var query = Flights.Where(f =>
        f.Status == "Landed" &&
        (SelectedAirport == null || f.DepartureAirport == SelectedAirport.IataCode));

        foreach (var flight in query)
        {
            FilteredFlights.Add(flight);
        }
    }

    [RelayCommand]
    private void FilterFlightsScheduled()
    {
        FilteredFlights.Clear();

        var query = Flights.Where(f =>
        f.Status == "Scheduled" &&
        (SelectedAirport == null || f.DepartureAirport == SelectedAirport.IataCode));

        foreach (var flight in query)
        {
            FilteredFlights.Add(flight);
        }
    }
}