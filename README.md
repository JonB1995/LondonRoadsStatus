# Road Status Checker

## Description
This console application checks the status of roads in London using the TfL API. 
Users can input a road ID and date range to get the road status.

## Prerequisites
- .NET 8 SDK
- An API key from TfL (Transport for London)

## How to Build
1. Clone the repository: `git clone https://github.com/JonB1995/LondonRoadsStatus`
2. Navigate to the project directory: `cd RoadStatusChecker`
3. Build the project: `dotnet build`

## How to Run
- Run the application with: `dotnet run --project LondonRoadsStatus [RoadID] [StartDate] [EndDate]`
- Example: `dotnet run --project RoadStatusChecker A2 "2022-11-10" "2022-11-18"`

## Running Tests
- Navigate to the test project directory: `cd LondonRoadsStatus`
- Run tests with: `dotnet test`

## Assumptions
- The application expects date inputs in the format "YYYY-MM-DD".
- If a date range is provided, both start and end dates must be included.
- Both the start and end date can not be more than 2 years in the past or more than 5 years in the future.

## Configuration
- Ensure your TfL API key is correctly set in `appsettings.json`.
- [Register For A Key](https://api-portal.tfl.gov.uk/).
