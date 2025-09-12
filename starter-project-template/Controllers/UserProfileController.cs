using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace starter_project_template.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetUserProfile()
        {
            throw new NotImplementedException();
        }
    }
}
