# HA3 - Flight Tracker

This is our third home assignment and it is a flight tracking app built with Avalonia UI and C#. The project uses the MVVM pattern to keep the code organized.

## Mockup
![Mockup of the project](PXL_20260410_145152892.jpg)
We dediavted a bit from original mockup, but with the goal to improve it or simplify the UI experience.

## Project Structure Diagram
### FlightTracker/                 
- Assets - Images (map pins) and the flights.json data file
- Models - Data classes like Airport.cs and Flight.cs and et cetera
- Services - Code that loads the data (FLightsAndAirportService.cs)
- ViewModels - Connects the UI to the data
- Views - The Avalonia UI screens (.axaml files)
- App.axaml - Main app styling
- Program.cs - The starting point of the app

### FlightTracker.Tests/
- FlightTracker.Tests - Folder for my unit tests

- README.md - thats what ur reading atm stupid

## Setup Instructions
To run this programm you gotta have .NET SDK installed. 
To run this programm use: 

dotnet run inside FlightTracker folder 

To test this build use: 

cd ../FlightTracker.Tests
dotnet test

## App components
The app is built using the MVVM (Model-View-ViewModel) pattern. This separates the visual parts from the logic and makes it easier to manage the code.
- Models (/Models): These are simple classes that hold data. For example Airport and Flight are models.
- Services (/Services): The FlightAndAiportService class does the heavy lifting. It reads the flight information from our flights.json file and prepares it for the app.
- ViewModels (/ViewModels): These act as the middleman between the Models and the Views. They hold the logic for different parts of the screen. For example RouteMapViewModel controls what happens on the map page.
- Views (/Views): These are the actual visual screens the user sees. They are written in Avalonia XAML (.axaml) and they show the data provided by the ViewModels.
