using Microsoft.Extensions.Configuration;

namespace LondonRoadsStatus
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            if (args.Length == 0 || args.Contains("-h") || args.Contains("--help"))
            {
                Console.WriteLine("Usage: dotnet run [RoadID] [StartDate] [EndDate]");
                Console.WriteLine("Example: dotnet run A2 2022-11-10 2022-11-18");
                return;
            }

            IConfigurationRoot configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            var apiKey = configuration["ApiKey"];
            if (string.IsNullOrEmpty(apiKey))
            {
                Console.WriteLine("API Key is missing in appsettings.json.");
                return;
            }

            // Take the parameters from the arguments - ID followed by start date and end date.
            var roadId = args[0];
            var startDate = args.Length > 2 ? args[1] : "";
            var endDate = args.Length > 2 ? args[2] : "";

            if (string.IsNullOrWhiteSpace(roadId))
            {
                Console.WriteLine("Road ID is required.");
                return;
            }

            var roadStatusService = new RoadsStatusService(apiKey);
            try
            {
                await roadStatusService.GetRoadStatus(roadId, startDate, endDate);
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex.Message);
                Environment.Exit(1);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error occurred: {ex.Message}");
                Environment.Exit(1);
            }
        }
    }
}