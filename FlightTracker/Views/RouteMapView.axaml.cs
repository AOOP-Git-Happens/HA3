using Avalonia.Controls;
using Mapsui.Tiling;
using Mapsui.Extensions;
using Mapsui.Widgets;
using Mapsui.Widgets.ScaleBar;
using Mapsui.Widgets.ButtonWidgets;
using System.Threading.Tasks;
using Mapsui.Widgets.InfoWidgets;
using Mapsui;
using FlightTracker.ViewModels;
using System;
using Mapsui.Layers;
using System.Collections;
using Mapsui.Projections;
using Mapsui.Styles;

namespace FlightTracker.Views;

public partial class RouteMapView : UserControl
{
    //stores airport marker layer, which can be removed later
    private MemoryLayer? _airportLayer;
    public RouteMapView()
    {
        InitializeComponent();
        MyMapControl.Map = CreateMap();

        //when view gets its view model, subscribe to event to redraw the map
        DataContextChanged += RouteMapView_DataContextChanged;
    }

    private void RouteMapView_DataContextChanged(object? sender, EventArgs e)
    {
        if (DataContext is RouteMapViewModel vm)
        {
            vm.MapUpdate += UpdateMap;
        }
    }

    private static Map CreateMap()
    {
        Map map = new Map
        {
            //CRS is Coordinate Reference System, projects earth in flat surface that map tiles, markers, routes
            CRS = "EPSG:3395" //resolution
        };

        //background OpenStreetMap tiles
        map.Layers.Add(OpenStreetMap.CreateTileLayer()); //map is not one big image, it split into small square images

        // Limit how far the user can zoom out.
        // Bigger values = more zoomed out.
        // By giving only a few allowed resolutions,
        // we stop the map from shrinking into a tiny box.
        map.Navigator.OverrideResolutions = new double[]
        {
            156543.033928,
            78271.516964,
            39135.758482,
            19567.879241,
            9783.9396205,
            4891.96981025,
            2445.98490513,
            1222.99245256,
            611.496226281,
            305.748113140
        };

        map.Widgets.Add(new ScaleBarWidget(map)
        {
            TextAlignment = Alignment.Center,
            HorizontalAlignment = Mapsui.Widgets.HorizontalAlignment.Center,
            VerticalAlignment = Mapsui.Widgets.VerticalAlignment.Top
        });

        map.Widgets.Add(new ZoomInOutWidget
        {
            Margin = new MRect(20, 50) //widget position
        });

        map.Widgets.Add(new MouseCoordinatesWidget());
        return map;
    }

    private void UpdateMap()
    {
        AddAirportPoint();
        //AddRouteLine();
    }
    //Point and MPoint

    //add one marker for selected airport
    private void AddAirportPoint()
    {
        if (DataContext is not RouteMapViewModel vm)
            return;

        if (vm.SelectedAirport == null)
            return;

        var selected = vm.SelectedAirport;

        // If an old point layer already exists, remove it first
        if (_airportLayer != null)
        {
            MyMapControl.Map?.Layers.Remove(_airportLayer);
        }

        // Create a new layer for the currently selected airport
        _airportLayer = CreatePointLayer(selected.Longitude, selected.Latitude);

        // Add the new layer to the map
        MyMapControl.Map?.Layers.Add(_airportLayer);

        //center map on selected airport
        var point = SphericalMercator.FromLonLat(selected.Longitude, selected.Latitude).ToMPoint();
        MyMapControl.Map?.Navigator.CenterOn(point);

        //set reasonable zoom level after selection
        //smaller index - more zoomed out
        if (MyMapControl.Map?.Navigator.OverrideResolutions is { Count: > 7 } resolutions)
        {
            MyMapControl.Map.Navigator.ZoomTo(resolutions[7]);
        }

        // Refresh the map so the new point becomes visible
        MyMapControl.Refresh();
    }

    private static MemoryLayer CreatePointLayer(double longitude, double latitude)
    {
        var point = SphericalMercator.FromLonLat(longitude, latitude).ToMPoint();
        var feature = new PointFeature(point);

        return new MemoryLayer
        {
            Name = "Points",
            Features = [feature],
            Style = new ImageStyle
            {
                Image = "embedded://FlightTracker.Assets.pin.png",
                SymbolScale = 0.05,

                //move pin up so the bottom tip points to the airport location
                Offset = new Offset(0, 225)
            }
        };
    }

    //LineString
    private void AddRouteLine()
    {
        if (DataContext is not RouteMapViewModel vm)
            return;

        var selected = vm.SelectedAirport;
        var destination = vm.DestinationAirports;

        if (selected == null)
            return;
        //loop through destinations
    }
}