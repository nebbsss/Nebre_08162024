using App.Core.Utils;
using App.Domain.Entities;
using App.Infrastructure.Repositories;
using CsvHelper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Globalization;

namespace App.Core.User;

public class UploadCsvCommand : IRequest<BaseResponse>
{
    private IFormFile? _csv;
    public IFormFile? Csv
    {
        get => _csv;
        set
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            if (value.Length <= 0) throw new ArgumentNullException(nameof(value));
            if (!Path.GetExtension(value.FileName).Equals(".csv")) throw new InvalidOperationException("File is not a valid CSV");
            
            _csv = value;
        }
    }
}

public class UploadCsvCommandHandler : BaseCommandHandler, IRequestHandler<UploadCsvCommand, BaseResponse>
{
    private readonly IRepository<CsvLogEntity> _csvLogRepository;
    private readonly IMediator _mediator;
    public UploadCsvCommandHandler(
        IMediator mediator,
        IRepository<CsvLogEntity> csvLogRepository,
        ILogger<UploadCsvCommandHandler> logger) : base(logger)
    {
        ArgumentNullException.ThrowIfNull(nameof(mediator));
        ArgumentNullException.ThrowIfNull(nameof(csvLogRepository));

        _csvLogRepository = csvLogRepository;
        _mediator = mediator;
    }

    public async Task<BaseResponse> Handle(UploadCsvCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse();

        try
        {
            var csv = request.Csv!;
            var fileSize = csv.Length;
            var fileName = csv.FileName;
            var duration = 0d;
            var recordsProcessed = 0;
            var totalRecords = 0;

            var stopwatch = Stopwatch.StartNew();

            using (var stream = new CsvReader(new StreamReader(csv.OpenReadStream()), CultureInfo.InvariantCulture))
            {
                var records = stream.GetRecords<CreateUserCommand>().ToList();
                totalRecords = records.Count();

                if (totalRecords <= 0)
                {
                    response.Success = false;
                    response.Message = "No records to processed!";
                    return response;
                }

                foreach (var record in records)
                {
                    var result = await _mediator.Send(record, cancellationToken).ConfigureAwait(false);
                    recordsProcessed = result.Success ? recordsProcessed++ : recordsProcessed;
                }
            }

            stopwatch.Stop();
            duration = stopwatch.ElapsedMilliseconds;

            var log = new CsvLogEntity()
            {
                DateCreated = DateTime.UtcNow,
                Duration = duration,
                FileName = fileName,
                FileSize = fileSize,
                RecordsProcessed = recordsProcessed,
                TotalRecords = totalRecords
            };

            await _csvLogRepository.Create(log, cancellationToken);

            response.Success = true;
            response.Message = "Csv file processed successfully";
        }
        catch (Exception ex)
        {
            LogError(ex, "Error upload csv file!");

            response.Success = false;
            response.Message = "Error! Try again.";
        }

        return response;
    }
}

