using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Api.Controllers;

[Route("api/[controller]/[action]")]
[Produces("application/json")]
[ApiController]
[Authorize]
public class BaseController : ControllerBase
{
}
