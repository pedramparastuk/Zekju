using Domain.Model.BaseModel;

namespace Domain.Model;

public class Subscription : IEntity<int>
{
    public int Id { get; set; }
    public int AgencyId { get; set; }
    public int OriginCityId { get; set; }
    public int DestinationCityId { get; set; }
}