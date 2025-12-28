using MediaControlApp.Domain.Models.Media;
using MediaControlApp.Domain.Models.Media.ValueObjects;

namespace MediaControlApp.Application.Services
{
    public interface IMediaService
    {
        Task<bool> Add(string title, string? description, Guid ganreId, DateTime publishedDate, Guid authorId, Rating? rating);
        Task<IEnumerable<Media>> GetAll();
        Task<IEnumerable<Media>> GetByAuthorId(Guid authorId);
        Task<IEnumerable<Media>> GetByGanreId(Guid ganreId);
        Task<Media?> GetById(Guid id);
        Task<IEnumerable<Media>> GetByMediaTypeId(Guid mediaTypeId);
        Task<Media?> GetByTitle(string title);
        Task<bool> Rate(Guid id, Rating rating);
        Task<bool> Remove(Guid id);
        Task<bool> SetConsumed(Guid id);
        Task<bool> Update(Guid id, string title, string? description, Guid ganreId, DateTime publishedDate, DateTime? lastConsumedDate, Guid authorId, Rating? rating);
    }
}