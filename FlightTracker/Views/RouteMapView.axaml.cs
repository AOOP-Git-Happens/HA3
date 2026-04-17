using Avalonia.Controls;
using Mapsui.Tiling;
using Mapsui.Extensions;
using Mapsui.Widgets;
using Mapsui.Widgets.ScaleBar;
using Mapsui.Widgets.ButtonWidgets;
using System.Threading.Tasks;
using Mapsui.Widgets.InfoWidgets;
using Mapsui;

namespace FlightTracker.Views;

public partial class RouteMapView : UserControl
{
    public RouteMapView()
    {
        InitializeComponent();
        MyMapControl.Map = CreateMap();
    }

    private static Map CreateMap()
    {
        Map map = new Map
        {
            //CRS is Coordinate Reference System, projects earth in flat surface that map tiles, markers, routes
            CRS = "EPSG:3395," //resolution
        };

        map.Layers.Add(OpenStreetMap.CreateTileLayer()); //map is not one big image, it split into small square images
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

    //Point and MPoint

    //LineString
}