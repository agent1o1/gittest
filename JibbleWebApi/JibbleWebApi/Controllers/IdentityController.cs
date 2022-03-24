using Data;
using Data.Services;
using JibbleWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace JibbleWebApi.Controllers
{
    [Route("identity")]
    public class IdentityController : ControllerBase
    {
        private readonly MoviesContext _moviesContext;
        private readonly IUserService _userService;
        private readonly IAuthenticationService _authenticationService;

        public IdentityController(MoviesContext moviesContext,
            IUserService userService,
            IAuthenticationService authenticationService)
        {
            _moviesContext = moviesContext;
            _userService = userService;
            _authenticationService = authenticationService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] LoginModel model)
        {
            if (string.IsNullOrEmpty(model.UserName) || string.IsNullOrEmpty(model.Password))
                return BadRequest("User name and password must be not empty");

            if (await _moviesContext.Users.AsQueryable().Where(x => x.UserName == model.UserName).AnyAsync())
                return BadRequest("Username already exist");

            var user = await _userService.CreateUser(model.UserName, model.Password);

            await _moviesContext.AddAsync(user);

            await _moviesContext.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (string.IsNullOrEmpty(model.UserName) || string.IsNullOrEmpty(model.Password))
                return BadRequest("User name and password must be not empty");

            var user = _moviesContext.Users.FirstOrDefault(x => x.UserName == model.UserName);

            if (user == null)
                return NotFound();

            if (!await _userService.CheckPassword(user, model.Password))
                return Forbid();
            
            var response = new
            {
                access_token = _authenticationService.GetAccessToken(user.ID),
            };

            return Ok(response);
        }

    }
}
