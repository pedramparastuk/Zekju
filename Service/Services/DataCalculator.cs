using Domain.Model;
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
        var data = await _context.Set<Subscription>()
            .Where(w => w.AgencyId == request.AgencyId)
            .Join(_context.Set<Route>().Include(r => r.Flights
                    .Where(w => w.DepartureTime.Date <= request.StartDate.AddDays(-7).Date)
                    .Where(w => w.ArrivalDate.Date >= request.EndDate.AddDays(7).Date)),
                s => new { s.DestinationCityId, s.OriginCityId },
                r => new { r.DestinationCityId, r.OriginCityId },
                (s, r) => r)
            .SelectMany(r => r.Flights, (r, f) => new { r, f })
            .OrderBy(o => o.f.DepartureTime)
            .ToListAsync(cancellationToken);

        var newData = data
            .Where(w => !data.Any(f => f.f.DepartureTime == w.f.DepartureTime.AddDays(-7)))
            .Select(s => s.f.Id);

        var discontinuedData = data
            .Where(w => !data.Any(f => f.f.DepartureTime == w.f.DepartureTime.AddDays(7)))
            .Select(s => s.f.Id);

        var finalData = data.Join(newData, d => d.f.Id, nd => nd, (d, nd) => new { d.r, d.f, status = "New" });
        
        var finalData2 = data.Join(discontinuedData, d => d.f.Id, dd => dd,
            (d, dd) => new { d.r, d.f, status = "Discount" });

        //DIstinct

        return finalData.Concat(finalData2).Where(w => w.f.DepartureTime.Date <= request.StartDate.Date)
            .Where(w => w.f.ArrivalDate.Date >= request.EndDate.Date)
            .Select(s => new ResponseDto
            {
                FlightId = s.f.Id,
                OriginCityId = s.r.OriginCityId,
                DestinationCityId = s.r.DestinationCityId,
                DepartureTime = s.f.DepartureTime,
                ArrivalTime = s.f.ArrivalDate,
                AirlineId = s.f.AirlineId,
                Status = s.status
            })
            .ToList();
    }
}