using CMS.Models;
namespace CMS.Business;

public interface IFavoriteBusiness{
    public int favorite(FavoriteInDto favoriteInDto);

    public int cancelFavorite(FavoriteInDto favoriteInDto);
}