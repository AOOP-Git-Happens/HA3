using Avalonia.Controls;
using FlightTracker.ViewModels;
namespace FlightTracker.Views;

/// <summary>
/// code-behind for main application window
/// initialise UI and assign ViewModel
/// </summary>

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainWindowViewModel();
    }
}