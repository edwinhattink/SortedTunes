using SortedTunes.Application.Artists.Commands.CreateArtist;
using SortedTunes.Application.Artists.Commands.DeleteArtist;
using SortedTunes.Application.Artists.Commands.UpdateArtist;
using SortedTunes.Application.Artists.Queries.GetArtists;
using SortedTunes.Application.Common.Models;

namespace SortedTunes.Web.Endpoints;

public class ArtistsEndpoint : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .RequireAuthorization()
            .MapGet(GetArtists) // Assuming you have a URL pattern
            .MapPost(CreateArtist)
            .MapPut(UpdateArtist, "{id}")
            .MapDelete(DeleteArtist, "{id}");
    }

    public async Task<PaginatedList<ArtistDto>> GetArtists(ISender sender)
    {
        return await sender.Send(new GetArtistsQuery());
    }

    public async Task<int> CreateArtist(ISender sender, CreateArtistCommand command)
    {
        return await sender.Send(command);
    }

    public async Task<IResult> UpdateArtist(ISender sender, int id, UpdateArtistCommand command)
    {
        if (id != command.Id) return Results.BadRequest();
        await sender.Send(command);
        return Results.NoContent();
    }

    public async Task<IResult> DeleteArtist(ISender sender, int id)
    {
        await sender.Send(new DeleteArtistCommand(id));
        return Results.NoContent();
    }
}
