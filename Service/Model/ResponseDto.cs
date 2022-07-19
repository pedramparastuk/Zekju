using System.Text.Json.Serialization;

namespace Service.Model;

public class ResponseDto
{
    [JsonPropertyName("flight_id")] public int FlightId { get; set; }
    [JsonPropertyName("origin_city_id")] public int OriginCityId { get; set; }

    [JsonPropertyName("destination_city_id")]
    public int DestinationCityId { get; set; }

    [JsonPropertyName("departure_time")] public DateTime DepartureTime { get; set; }
    [JsonPropertyName("arrival_time")] public DateTime ArrivalTime { get; set; }
    [JsonPropertyName("airline_id")] public int AirlineId { get; set; }
    [JsonPropertyName("status")] public string Status { get; set; }
}