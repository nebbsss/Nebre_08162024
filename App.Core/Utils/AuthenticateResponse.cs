namespace App.Core.Utils;

public class AuthenticateResponse : BaseResponse
{
    public string? JwtToken { get; set; }
}
