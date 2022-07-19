using Service.Model;

namespace Service.Services;

public interface IDataCalculator
{
    Task<IList<ResponseDto>> GetFlights(RequestDto request, CancellationToken cancellationToken);
}