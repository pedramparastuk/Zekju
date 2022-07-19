using Domain.Model.BaseModel;

namespace Domain.Model;

public class Route : IEntity<int>
{
    public int Id { get; set; }
    public int OriginCityId { get; set; }
    public int DestinationCityId { get; set; }
    public DateTime DepartureDate { get; set; }

    public virtual IList<Flight> Flights { get; set; }
}