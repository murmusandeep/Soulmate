using Microsoft.AspNetCore.Mvc;
using Soulmate.Helper;

namespace Soulmate.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [Route("api/[controller]")]
    [ApiController]
    public class BaseApiController : ControllerBase
    {
    }
}
