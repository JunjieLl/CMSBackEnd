using CMS.Models;
using AutoMapper;

namespace CMS.Business;

class FavoriteBusiness : IFavoriteBusiness
{
    private readonly cmsContext context;
    private readonly IMapper mapper;

    public FavoriteBusiness(cmsContext context, IMapper mapper)
    {
        this.mapper = mapper;
        this.context = context;
    }

    public int favorite(FavoriteInDto favoriteInDto)
    {
        var favorites = context.Favorites
        .Where(f => f.RoomId.Equals(favoriteInDto.RoomId) && f.UserId.Equals(favoriteInDto.UserId))
        .ToList();
        if (favorites.Count == 1)
        {
            return -1;
        }

        if (null == context.CommonUsers.Single(u => u.UserId.Equals(favoriteInDto.UserId)))
        {
            return -1;
        }

        context.Favorites.Add(mapper.Map<Favorite>(favoriteInDto));
        context.SaveChanges();
        return 1;
    }

    public int cancelFavorite(FavoriteInDto favoriteInDto)
    {
        if (null == context.CommonUsers.Single(u => u.UserId.Equals(favoriteInDto.UserId)))
        {
            return -1;
        }

        var favorites = context.Favorites
        .Where(f => f.RoomId.Equals(favoriteInDto.RoomId) && f.UserId.Equals(favoriteInDto.UserId))
        .ToList();

        if (favorites.Count != 1)
        {
            return -1;
        }

        context.Favorites.Remove(favorites[0]);
        context.SaveChanges();
        return 1;
    }
}