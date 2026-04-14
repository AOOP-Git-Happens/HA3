using System;
using CommunityToolkit.Mvvm.ComponentModel;

using FlightTracker.Models;
using FlightTracker.Services;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Linq;
namespace FlightTracker.ViewModels;

/// <summary>
/// ViewModel 1. Contains airport search, map integration,
/// selected airports, route visualisation, clear/reset
/// </summary>

public partial class RouteMapViewModel : ViewModelBase
{
    [ObservableProperty]
    private string searchText = "";
    partial void OnSearchTextChanged(string value) //generated partial method when property changes
    {
        FilterAirports();
    }
    
    [ObservableProperty]
    private Airport? selectedAirport;

    public ObservableCollection<Airport> Airports { get; } = new();
    public ObservableCollection<Airport> FilteredAirports { get; } = new(); 

    public RouteMapViewModel()
    {
        Header = "Map"; //tab name

        //samples
        Airports.Add(new Airport { IataCode = "CPH", Name = "Copenhagen Airport", City = "Copenhagen", Country = "Denmark"});
        Airports.Add(new Airport { IataCode = "TLL", Name = "Tallinn Airport", City = "Tallinn", Country = "Estonia"});
        Airports.Add(new Airport { IataCode = "RIA", Name = "Riga Airport", City = "Riga", Country = "Latvia"});

        FilterAirports();
    }

    private void FilterAirports()
    {
        FilteredAirports.Clear();

        var filtered = string.IsNullOrWhiteSpace(SearchText)
            ? Airports
            : new ObservableCollection<Airport>(
                Airports.Where(a =>
                    a.Name.Contains(SearchText, System.StringComparison.OrdinalIgnoreCase) ||
                    a.City.Contains(SearchText, System.StringComparison.OrdinalIgnoreCase) ||
                    a.IataCode.Contains(SearchText, System.StringComparison.OrdinalIgnoreCase)));

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
        FilteredAirports.Clear();

        foreach (var airport in Airports)
        {
            FilteredAirports.Add(airport);
        }
    }
}

