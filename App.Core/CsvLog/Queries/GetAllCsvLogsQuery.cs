using App.Core.Utils;
using App.Domain.Entities;
using App.Infrastructure.Dtos;
using App.Infrastructure.Repositories;
using App.Infrastructure.Transformers;
using MediatR;
using Microsoft.Extensions.Logging;

namespace App.Core.CsvLog.Queries;

public class GetAllCsvLogsQuery : IRequest<IEnumerable<CsvLogDto>>
{

}

public class GetAllCsvLogsQueryHandler : BaseCommandHandler, IRequestHandler<GetAllCsvLogsQuery, IEnumerable<CsvLogDto>>
{
    private readonly IRepository<CsvLogEntity> _csvLogRepository;
    public GetAllCsvLogsQueryHandler(IRepository<CsvLogEntity> csvLogRepository, ILogger<GetAllCsvLogsQueryHandler> logger) : base(logger)
    {
        ArgumentNullException.ThrowIfNull(nameof(csvLogRepository));
        _csvLogRepository = csvLogRepository;
    }

    public async Task<IEnumerable<CsvLogDto>> Handle(GetAllCsvLogsQuery request, CancellationToken cancellationToken)
    {
        return (await _csvLogRepository.GetAllAsync(cancellationToken)).Select(u => u.ToDto());
    }
}
