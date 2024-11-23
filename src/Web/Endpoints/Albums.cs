using SortedTunes.Application.Albums.Commands.CreateAlbum;
using SortedTunes.Application.Albums.Commands.DeleteAlbum;
using SortedTunes.Application.Albums.Commands.UpdateAlbum;
using SortedTunes.Application.Albums.Queries;
using SortedTunes.Application.Albums.Queries.GetAlbums;
using SortedTunes.Application.Common.Models;

namespace SortedTunes.Web.Endpoints;

public class Albums : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            //.RequireAuthorization()
            .MapGet(GetAlbums)
            .MapPost(CreateAlbum)
            .MapGet(GetAlbum, "{id}")
            .MapPut(UpdateAlbum, "{id}")
            .MapDelete(DeleteAlbum, "{id}");
    }

    public async Task<PaginatedList<AlbumDto>> GetAlbums(ISender sender)
    {
        return await sender.Send(new GetAlbumsQuery());
    }

    public async Task<int> CreateAlbum(ISender sender, CreateAlbumCommand command)
    {
        return await sender.Send(command);
    }

    public async Task<AlbumDto> GetAlbum(ISender sender, int id)
    {
        return await sender.Send(new GetAlbumQuery { Id = id });
    }

    public async Task<IResult> UpdateAlbum(ISender sender, int id, UpdateAlbumCommand command)
    {
        if (id != command.Id) return Results.BadRequest();
        await sender.Send(command);
        return Results.NoContent();
    }

    public async Task<IResult> DeleteAlbum(ISender sender, int id)
    {
        await sender.Send(new DeleteAlbumCommand(id));
        return Results.NoContent();
    }
}
