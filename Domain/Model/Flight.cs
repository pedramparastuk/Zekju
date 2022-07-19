using Domain.Model.BaseModel;

namespace Domain.Model;

public class Flight : IEntity<int>
{
    public int Id { get; set; }
    public int RouteId { get; set; }
    public DateTime DepartureTime { get; set; }
    public DateTime ArrivalDate { get; set; }
    public int AirlineId { get; set; }

    public virtual Route Route { get; set; }
}