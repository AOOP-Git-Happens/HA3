using Avalonia.Controls;
using Mapsui.Tiling;

namespace FlightTracker.Views;

public partial class RouteMapView : UserControl
{
    public RouteMapView()
    {
        InitializeComponent();
        MyMapControl.Map?.Layers.Add(OpenStreetMap.CreateTileLayer());
    }
}