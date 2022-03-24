using System;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Data.Entities;
using JibbleWebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JibbleWebApi.Controllers
{
    [Route("favorites")]
    public class FavoritesController : ControllerBase
    {
        private readonly MoviesContext _context;

        public FavoritesController(MoviesContext context)
        {
            _context = context;
        }

        [HttpPost("add")]
        [Authorize]
        public async Task<IActionResult> AddToMyFavorites([FromBody] FavoriteModel[] favorites)
        {
            var userId = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            if (userId == 0)
                return Unauthorized();

            foreach (var favorite in favorites)
                if (!await _context.Favorites.Where(x => x.User.ID == userId).Where(x => x.ImdbID == favorite.ImdbID)
                    .AnyAsync())
                    _context.Add(new Favorites()
                    {
                        UserId = userId,
                        ImdbID = favorite.ImdbID,
                        Title = favorite.Title,
                        Year = favorite.Year
                    });

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("remove")]
        [Authorize]
        public async Task<IActionResult> RemoveFromMyFavorites([FromBody] string[] ids)
        {
            var userId = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            if (userId == 0)
                return Unauthorized();

            foreach (var id in ids)
                if (await _context.Favorites
                    .Where(x => x.User.ID == userId)
                    .Where(x => x.ImdbID == id)
                    .Select(x => x.ID)
                    .FirstOrDefaultAsync() is { } favoriteId)
                {
                    var toRemove = new Favorites { ID = favoriteId };
                    _context.Attach(toRemove);
                    _context.Favorites.Remove(toRemove);
                }

            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}