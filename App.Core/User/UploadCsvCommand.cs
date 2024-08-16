using App.Core.Utils;
using CsvHelper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
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
    private readonly IMediator _mediator;
    public UploadCsvCommandHandler(
        IMediator mediator,
        ILogger<UploadCsvCommandHandler> logger) : base(logger)
    {
        ArgumentNullException.ThrowIfNull(nameof(mediator));
        _mediator = mediator;
    }

    public async Task<BaseResponse> Handle(UploadCsvCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse();

        try
        {
            using (var csv = new CsvReader(new StreamReader(request.Csv!.OpenReadStream()), CultureInfo.InvariantCulture))
            {
                var csvRecords = csv.GetRecords<CreateUserCommand>();

                foreach (var record in csvRecords)
                {
                    await _mediator.Send(record, cancellationToken).ConfigureAwait(false);
                }
                
            }

            response.Success = true;
            response.Message = "User successfully created!";
        }
        catch (Exception ex)
        {
            LogError(ex, "Error create user");

            response.Success = false;
            response.Message = "Error! Try again.";
        }

        return response;
    }
}

