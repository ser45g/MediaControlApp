using MediaControlApp.Domain.Models.Media;
using MediaControlApp.Domain.Models.Media.ValueObjects;

namespace MediaControlApp.Application.Services
{
    public interface IMediaService
    {
        Task<bool> Add(string title, string? description, Guid ganreId, DateTime publishedDate, Guid authorId, Rating? rating, CancellationToken cancellationToken = default);
        Task<IEnumerable<Media>> GetAll(CancellationToken cancellationToken = default);
        Task<IEnumerable<Media>> GetByAuthorId(Guid authorId, CancellationToken cancellationToken = default);
        Task<IEnumerable<Media>> GetByGanreId(Guid ganreId, CancellationToken cancellationToken = default);
        Task<Media?> GetById(Guid id, CancellationToken cancellationToken = default);
        Task<IEnumerable<Media>> GetByMediaTypeId(Guid mediaTypeId, CancellationToken cancellationToken = default);
        Task<Media?> GetByTitle(string title, CancellationToken cancellationToken = default);
        Task<bool> Rate(Guid id, Rating rating, CancellationToken cancellationToken = default);
        Task<bool> Remove(Guid id, CancellationToken cancellationToken = default);
        Task<bool> SetConsumed(Guid id, DateTime lastConsumed, CancellationToken cancellationToken = default);
        Task<bool> Update(Guid id, string title, string? description, Guid ganreId, DateTime publishedDate, DateTime? lastConsumedDate, Guid authorId, Rating? rating, CancellationToken cancellationToken = default);
    }
}