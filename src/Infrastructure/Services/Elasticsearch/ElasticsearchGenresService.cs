using Elastic.Clients.Elasticsearch;

using SortedTunes.Application.Common.Models;
using SortedTunes.Application.Elasticsearch.Interfaces;
using SortedTunes.Application.Elasticsearch.Models;
using SortedTunes.Infrastructure.Services.Elasticsearch.Base;

namespace SortedTunes.Infrastructure.Services.Elasticsearch;

public class ElasticsearchGenresService(ElasticsearchClient elasticsearchClient, TimeProvider timeProvider)
    : BaseElasticsearchService<GenreDocument>(elasticsearchClient, timeProvider), IElasticsearchGenresService
{
    protected override string IndexPrefix => "sorted-tunes-genres";

    public Task IndexGenreAsync(GenreDocument genre, CancellationToken cancellationToken)
        => IndexDocumentInternalAsync(genre, genre.Id, cancellationToken);

    public Task IndexGenresAsync(IEnumerable<GenreDocument> genres, CancellationToken cancellationToken)
        => IndexDocumentsInternalAsync(genres, g => g.Id, cancellationToken);

    public Task ResetIndexAsync(CancellationToken cancellationToken)
        => ResetIndexInternalAsync(cancellationToken);

    public Task<PaginatedList<GenreDocument>> SearchGenresAsync(int pageNumber, int pageSize, string? search = null, CancellationToken cancellationToken = default)
        => SearchInternalAsync(pageNumber, pageSize, search, cancellationToken);

    public Task DeleteGenreAsync(int genreId, CancellationToken cancellationToken)
        => DeleteDocumentInternalAsync(genreId, cancellationToken);

    protected override IEnumerable<Field> GetSearchFields()
    {
        yield return new Field("name");
        yield return new Field("name.search_as_you_type");
    }
}
