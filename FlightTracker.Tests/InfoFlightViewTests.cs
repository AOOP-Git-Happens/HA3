using FlightTracker.ViewModels;
using Avalonia.Headless; // enables UI testing without opening real windows
using Avalonia.Headless.XUnit; // integrates Avalonia with xUnit
using FluentAssertions;
using xUnit;

namespace FlightTracker.Tests;

// Test cases that cover at least ViewModel logic and LINQ query results

public class InfoFlightViewModelTests
{
    public readonly InfoFlightViewModel _infoFlightViewModel;
    
    public InfoFlightViewModelTests()
    {
        // SUT (System Under Test)
        _infoFlightViewModel = new InfoFlightViewModel();
    }

    [Fact]
    public void InfoFlightViewModel_ClearSelection_SetsNull()
    {
        //Arrange

        //Act
        var result = _infoFlightViewModel.ClearSelection();

        //Assert
        result.Should.BeNull();
    }
}