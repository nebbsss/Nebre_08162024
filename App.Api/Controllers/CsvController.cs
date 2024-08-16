using App.Core.Authenticate.Commands;
using App.Core.CsvLog.Queries;
using App.Core.User.Commands;
using App.Core.User.Queries;
using App.Infrastructure.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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

    [HttpPost, AllowAnonymous]
    public async Task<IActionResult> Authenticate(AuthenticateCommand command, CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(command, cancellationToken).ConfigureAwait(false);
        if (result.Success)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpGet]
    public async Task<IEnumerable<UserDto>> GetUsers(CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new GetAllUsersQuery(), cancellationToken).ConfigureAwait(false);
    }

    [HttpGet]
    public async Task<IEnumerable<CsvLogDto>> GetCsvLogs(CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new GetAllCsvLogsQuery(), cancellationToken).ConfigureAwait(false);
    }
}
