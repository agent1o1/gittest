using Data;
using JibbleWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Data.Services;
using Data.Entities;

namespace JibbleWebApi.Controllers
{
    [Route("login")]
    public class LoginController : ControllerBase
    {
        private readonly MoviesContext _moviesContext;
        private readonly IUserService _userService;

        public LoginController(MoviesContext moviesContext, IUserService userService)
        {
            _moviesContext = moviesContext;
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (!string.IsNullOrEmpty(model.UserName) || !string.IsNullOrEmpty(model.Password))
                return BadRequest("User name and password must be not empty");

            if (await _moviesContext.Users.AsQueryable().Where(x => x.UserName == model.UserName).AnyAsync())
                return BadRequest("Username already exist");

            var salt = _userService.GenerateSalt();
            var user = new User { Password = _userService.GeneratePasswordHash(model.Password, salt), UserName = model.UserName };

            await _moviesContext.AddAsync(user);

            await _moviesContext.SaveChangesAsync();

            return Ok();
        }
    }
}
