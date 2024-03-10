
//using Microsoft.AspNetCore.Mvc;
//using TestiranjeProjekat.Service;

//namespace TestiranjeProjekat.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    //[Route("api/User")]
//    public class UserController : ControllerBase
//    {
//        private readonly IUserService _userService;
//        public UserController(IUserService userService)
//        {
//            _userService = userService;
//        }
//        [HttpGet("users")]
//        public IActionResult GetAllUsers()
//        {
//            var users = _userService.GetAllUsers();
//            return Ok(users);

//        }
//    }
//}