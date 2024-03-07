using Newtonsoft.Json;
using System.Net;

namespace LondonRoadsStatus
{
    public class RoadsStatusService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public RoadsStatusService(string apiKey, HttpClient? httpClient = null)
        {
            _httpClient = httpClient ?? new HttpClient();
            _apiKey = apiKey;
        }

        public async Task<string> GetRoadStatus(string roadId, string startDate, string endDate)
        {
            var url = $"https://api.tfl.gov.uk/Road/{roadId}/Status?app_key={_apiKey}";

            // If the date range is valid, append it to the URL.
            var isValidDateRange = (
                DateTime.TryParse(startDate, out DateTime startDt) 
                && DateTime.TryParse(endDate, out DateTime endDt) 
                && endDt > startDt
                // The dates can not be more than 2 years in the past or more than 5 years in the future.
                && startDt > DateTime.Today.AddYears(-2)
                && endDt > DateTime.Today.AddYears(-2)
                && startDt < DateTime.Today.AddYears(5)
                && endDt < DateTime.Today.AddYears(5)
            );

            if (isValidDateRange)
            {
                url += $"&startDate={Uri.EscapeDataString(startDate)}&endDate={Uri.EscapeDataString(endDate)}";
            };

            string result = "";
            
            var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var roadStatuses = JsonConvert.DeserializeObject<List<RoadStatus>>(content);

                // Build the return string based on the data returned.
                foreach (var status in roadStatuses)
                {
                    result += $"{status.DisplayName}\n";

                    // Only return the ID if no date range was provided.
                    if (isValidDateRange)
                    {
                        result += $"Road Status: {status.StatusSeverity}\n";
                        result += $"Road Status Description: {status.StatusSeverityDescription}\n";
                    }
                }
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();

                switch (response.StatusCode)
                {
                    case HttpStatusCode.BadRequest:
                        // Handle bad request errors specifically
                        throw new Exception($"Bad Request: {errorContent}");
                    case HttpStatusCode.InternalServerError:
                        // Handle internal server errors specifically
                        throw new Exception($"Server Error: {errorContent}");
                    default:
                        // Handle other types of errors
                        throw new HttpRequestException($"HTTP Error: {errorContent}");
                }
            }

            Console.WriteLine(result);
            return result;
        }
    }
}
