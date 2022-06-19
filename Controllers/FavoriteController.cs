using CMS.Models;
using CMS.Business;
using Microsoft.AspNetCore.Mvc;
using CMS.Business;

namespace CMS.Controllers;

[ApiController]
[Route("api/Favorite")]
public class FavoriteController : ControllerBase
{
    private readonly IFavoriteBusiness favoriteBusiness;

    public FavoriteController(IFavoriteBusiness favoriteBusiness)
    {
        this.favoriteBusiness = favoriteBusiness;
    }
    [HttpPost]
    public IActionResult favorite(FavoriteInDto favoriteInDto)
    {
        int res = favoriteBusiness.favorite(favoriteInDto);
        if (-1 == res)
        {
            return BadRequest();
        }
        return Created(nameof(favorite), favoriteInDto);
    }

    [HttpPost("delete")]
    public IActionResult cancelFavorite(FavoriteInDto favoriteInDto)
    {
        int res = favoriteBusiness.cancelFavorite(favoriteInDto);
        if (-1 == res)
        {
            return BadRequest();
        }
        return Ok();
    }
}


