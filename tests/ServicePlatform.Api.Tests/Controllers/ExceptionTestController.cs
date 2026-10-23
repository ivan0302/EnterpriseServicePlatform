
using Microsoft.AspNetCore.Mvc;

namespace ServicePlatform.Api.Tests.Controllers;

[ApiController]
[Route("_test/exceptions")]
public sealed class ExceptionTestController : ControllerBase
{
    [HttpGet("throw")]
    public IActionResult ThrowException()
    {
        throw new InvalidOperationException(
            "TEST-ONLY-SECRET: simulated internal failure");
    }
}
