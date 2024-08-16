using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace App.Infrastructure.Services;

public class JwtService : IJwtService
{
    private readonly string _jwtSecret;
    private readonly string _jwtAudience;
    private readonly string _jwtIssuer;
    private readonly string _jwtDurationInMinutes;

    public JwtService(IConfiguration configuration)
    {
        _jwtSecret = configuration["JWT:Secret"] ?? throw new ArgumentNullException(nameof(_jwtSecret));
        _jwtAudience = configuration["JWT:ValidAudience"] ?? throw new ArgumentNullException(nameof(_jwtAudience));
        _jwtIssuer = configuration["JWT:ValidIssuer"] ?? throw new ArgumentNullException(nameof(_jwtIssuer));
        _jwtDurationInMinutes = configuration["JWT:DurationInMinutes"] ?? throw new ArgumentNullException(nameof(_jwtDurationInMinutes));
    }

    public string CreateToken()
    {
        var key = Encoding.ASCII.GetBytes(_jwtSecret);
        var signKey = new SymmetricSecurityKey(key);

        var userClaims = BuildClaims();
        var jwtSecurityToken = new JwtSecurityToken(
            issuer: _jwtIssuer,
            notBefore: DateTime.UtcNow,
            audience: _jwtAudience,
            expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(_jwtDurationInMinutes)),
            claims: userClaims,
            signingCredentials: new SigningCredentials(signKey, SecurityAlgorithms.HmacSha256));

        return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
    }

    private List<Claim> BuildClaims()
    {
        return new List<Claim>()
        {
            new Claim(ClaimTypes.Email, "testing@email.com"),
            new Claim(ClaimTypes.Name, "testing@email.com"),
            new Claim(ClaimTypes.GivenName, "Test First Name"),
            new Claim(ClaimTypes.Surname, "Test Last Name"),
            new Claim(ClaimTypes.NameIdentifier, "123456789")
        };
    }
}
