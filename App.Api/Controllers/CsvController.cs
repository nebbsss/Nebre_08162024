using App.Core.User;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace App.Api.Controllers;

public class CsvController : BaseController
{
    private readonly IMediator _mediator;

    public CsvController(IMediator mediator)
    {
        ArgumentNullException.ThrowIfNull(nameof(mediator));
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Upload(UploadCsvCommand command, CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(command, cancellationToken).ConfigureAwait(false);
        if (result.Success)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }
}
