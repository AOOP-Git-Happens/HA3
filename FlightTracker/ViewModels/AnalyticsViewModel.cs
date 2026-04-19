using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using System.Linq;
using System.Collections.Generic;
using FlightTracker.Models;

namespace FlightTracker.ViewModels;

/// <summary>
/// ViewModel 3. LINQ analytics, charts, export.
/// </summary>

public partial class AnalyticsViewModel : ViewModelBase
{
    
    public AnalyticsViewModel()
    {
        Header = "Analytics"; // tab name
    }

    [RelayCommand]
    private void Export()
    {
        // To be implemented later
    }

    [ObservableProperty]
    private DateTimeOffset? _SelectedDate;

            public ISeries[] Series { get; set; }
            = new ISeries[]
            {
                new PieSeries<double> { Values = new double[] { 2 } },
                new PieSeries<double> { Values = new double[] { 4 } },
                new PieSeries<double> { Values = new double[] { 1 } },
                new PieSeries<double> { Values = new double[] { 4 } },
                new PieSeries<double> { Values = new double[] { 3 } }
            };
}