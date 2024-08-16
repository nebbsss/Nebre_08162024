using App.Core.Utils;
using App.Infrastructure.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace App.Core.Authenticate.Commands;

public class AuthenticateCommand : IRequest<AuthenticateResponse>
{
    private string? _userName;
    private string? _password;
    public string? UserName
    {
        get => _userName;
        set
        {
            if(String.IsNullOrWhiteSpace(value)) throw new ArgumentNullException(nameof(value));
            _userName = value;
        }
    }
    public string? Password
    {
        get => _password;
        set
        {
            if (String.IsNullOrWhiteSpace(value)) throw new ArgumentNullException(nameof(value));
            _password = value;
        }
    }
}

public class AuthenticateCommandHandler : BaseCommandHandler, IRequestHandler<AuthenticateCommand, AuthenticateResponse>
{

    private readonly IJwtService _jwtService;
    public AuthenticateCommandHandler(IJwtService jwtService, ILogger<AuthenticateCommandHandler> logger) : base(logger)
    {
        ArgumentNullException.ThrowIfNull(nameof(jwtService));
        _jwtService = jwtService;
    }

    public async Task<AuthenticateResponse> Handle(AuthenticateCommand request, CancellationToken cancellationToken)
    {
        var response = new AuthenticateResponse();

        if(request.UserName!.Equals("admin", StringComparison.OrdinalIgnoreCase) &&
          request.Password!.Equals("admin", StringComparison.OrdinalIgnoreCase))
        {
            response.Success = true;
            response.JwtToken = _jwtService.CreateToken();
        }
        else
        {
            response.Success = false;
            response.JwtToken = "Invalid credentials!";
        }

        return  await Task.FromResult(response);
    }
}
