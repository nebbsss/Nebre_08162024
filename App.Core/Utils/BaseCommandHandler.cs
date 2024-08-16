using Microsoft.Extensions.Logging;

namespace App.Core.Utils;

public class BaseCommandHandler
{
    private readonly ILogger _logger;

    public BaseCommandHandler(ILogger logger)
    {
        ArgumentNullException.ThrowIfNull(nameof(logger));
        _logger = logger;
    }

    public void LogError(Exception? exception, string? message)
    {
        _logger.LogError(exception, message);
    }

    public void LogInformation(string? message)
    {
        _logger.LogInformation(message);
    }
}