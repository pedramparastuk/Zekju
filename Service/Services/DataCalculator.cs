using Microsoft.EntityFrameworkCore;
using Service.Model;
using AppContext = Infrastructure.AppContext;

namespace Service.Services;

public class DataCalculator : IDataCalculator
{
    private readonly AppContext _context;

    public DataCalculator(AppContext context)
    {
        _context = context;
    }

    public async Task<IList<ResponseDto>> GetFlights(RequestDto request, CancellationToken cancellationToken)
    {
        var subscriptions = _context.Subscriptions;
        var flights = _context.Flights;
        var routes = _context.Routes;

        var result = await subscriptions
            .Where(w => w.AgencyId == request.AgencyId)
            .Join(routes.Join(flights
                        .Where(w => w.DepartureTime.Date >= request.StartDate.Date)
                        .Where(w => w.DepartureTime.Date <= request.EndDate.Date),
                    r => r.Id,
                    f => f.RouteId,
                    (route, flight) => new { Route = route, Flight = flight }),
                s => new { s.DestinationCityId, s.OriginCityId },
                rf => new { rf.Route.DestinationCityId, rf.Route.OriginCityId },
                (s, rf) => new
                {
                    rf.Route.DestinationCityId,
                    rf.Route.OriginCityId,
                    rf.Flight.Id,
                    rf.Flight.AirlineId,
                    rf.Flight.DepartureTime,
                    rf.Flight.ArrivalDate,
                })
            .Select(s => new
            {
                s.Id,
                s.DestinationCityId,
                s.OriginCityId,
                s.AirlineId,
                s.DepartureTime,
                s.ArrivalDate,

                HasNext = flights.Where(w =>
                        w.DepartureTime.Date >= request.StartDate.Date && w.DepartureTime.Date <= request.EndDate.Date)
                    .Where(w => s.OriginCityId == w.Route.OriginCityId &&
                                s.DestinationCityId == w.Route.DestinationCityId)
                    .Any(w => w.AirlineId == s.AirlineId && s.DepartureTime.AddDays(7) == w.DepartureTime),

                HasPrev = flights.Where(w =>
                        w.DepartureTime.Date >= request.StartDate.Date && w.DepartureTime.Date <= request.EndDate.Date)
                    .Where(w => s.OriginCityId == w.Route.OriginCityId &&
                                s.DestinationCityId == w.Route.DestinationCityId)
                    .Any(w => w.AirlineId == s.AirlineId && s.DepartureTime.AddDays(-7) == w.DepartureTime)
            })
            .Select(s => new ResponseDto
            {
                FlightId = s.Id,
                OriginCityId = s.OriginCityId,
                DestinationCityId = s.DestinationCityId,
                DepartureTime = s.DepartureTime,
                ArrivalTime = s.ArrivalDate,
                AirlineId = s.AirlineId,
                Status = s.HasPrev ? s.HasNext ? "" : "Discontinued" : "New"
            })
            .ToListAsync(cancellationToken);

        return result;
    }
}