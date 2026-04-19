using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using System.Linq;
using FlightTracker.Models;
using FlightTracker.Services;

namespace FlightTracker.ViewModels;

/// <summary>
/// ViewModel 3. LINQ analytics, charts, export.
/// </summary>
public partial class AnalyticsViewModel : ViewModelBase
{
    private readonly FlightAndAirportService _service;

    public AnalyticsViewModel()
    {
        Header = "Analytics"; // tab name
        _service = new FlightAndAirportService(); //load the data
    }

    [ObservableProperty]
    private DateTimeOffset? _selectedDate;

    // pii chart
    [ObservableProperty]
    private ISeries[] _series = Array.Empty<ISeries>();

    // colum chart
    [ObservableProperty]
    private ISeries[] _routeSeries = Array.Empty<ISeries>();

    [ObservableProperty]
    private Axis[] _routeXAxes = Array.Empty<Axis>();

    // busiest airport
    [ObservableProperty]
    private ISeries[] _busiestAirportSeries = Array.Empty<ISeries>();

    [ObservableProperty]
    private Axis[] _busiestAirportXAxes = Array.Empty<Axis>();

    partial void OnSelectedDateChanged(DateTimeOffset? value)
    {
        if (value.HasValue)
        {
            // pie
            Series = _service.Flights
                .Where(f => f.ScheduledDeparture.Date == value.Value.Date)
                .GroupBy(f => f.AirlineName)
                .Select(group => new PieSeries<int>
                {
                    Name = group.Key,
                    Values = new int[] { group.Count() }
                })
                .ToArray();

            // popular route
            var topRoutes = _service.Flights
                .Where(f => f.ScheduledDeparture.Date == value.Value.Date)
                .GroupBy(f => f.DepartureAirport + " ➔ " + f.ArrivalAirport)
                .Select(group => new { RouteName = group.Key, FlightCount = group.Count() })
                .OrderByDescending(r => r.FlightCount)
                .Take(5)
                .ToArray();

            RouteSeries = new ISeries[]
            {
                new ColumnSeries<int>
                {
                    Name = "Flights",
                    Values = topRoutes.Select(r => r.FlightCount).ToArray()
                }
            };

            RouteXAxes = new Axis[]
            {
                new Axis
                {
                    Labels = topRoutes.Select(r => r.RouteName).ToArray()
                }
            };

            // busiest airport
            var topAirports = _service.Flights
                .Where(f => f.ScheduledDeparture.Date == value.Value.Date)
                .GroupBy(f => f.DepartureAirport)
                .Select(group => new { AirportCode = group.Key, FlightCount = group.Count() })
                .OrderByDescending(a => a.FlightCount)
                .Take(5)
                .ToArray();

            BusiestAirportSeries = new ISeries[]
            {
                new ColumnSeries<int>
                {
                    Name = "Departures",
                    Values = topAirports.Select(a => a.FlightCount).ToArray()
                }
            };

            BusiestAirportXAxes = new Axis[]
            {
                new Axis
                {
                    Labels = topAirports.Select(a => a.AirportCode).ToArray()
                }
            };
        }
        else
        {
            Series = Array.Empty<ISeries>();
            RouteSeries = Array.Empty<ISeries>();
            RouteXAxes = Array.Empty<Axis>();
            BusiestAirportSeries = Array.Empty<ISeries>();
            BusiestAirportXAxes = Array.Empty<Axis>();
        }
    }

   [RelayCommand]
    private void Export()
    {
        if (!SelectedDate.HasValue) return;

        var date = SelectedDate.Value.Date;
        var dailyFlights = _service.Flights.Where(f => f.ScheduledDeparture.Date == date).ToList();

        var exportData = new
        {
            Date = date.ToShortDateString(),
            TotalFlights = dailyFlights.Count,
            Airlines = dailyFlights.GroupBy(f => f.AirlineName).Select(g => new { Name = g.Key, Count = g.Count() }),
            Routes = dailyFlights.GroupBy(f => f.DepartureAirport + " ➔ " + f.ArrivalAirport).Select(g => new { Route = g.Key, Count = g.Count() }).Take(5),
            Departures = dailyFlights.GroupBy(f => f.DepartureAirport).Select(g => new { Airport = g.Key, Count = g.Count() }).Take(5)
        };

        var options = new System.Text.Json.JsonSerializerOptions { WriteIndented = true };
        string jsonString = System.Text.Json.JsonSerializer.Serialize(exportData, options);

        string fullPath = @"Assets\analytic_export.json";

        System.IO.File.WriteAllText(fullPath, jsonString);
    }
}
