


using MediaControlApp.Domain.Models.Media;
using MediaControlApp.Domain.Models.Media.ValueObjects;

namespace MediaControlApp.Application.Services.Interfaces
{
    public interface IMediaRepo
    {
        Task<bool> Add(string title,  Guid ganreId, DateTime publishedDate, Guid authorId,string? description=null, DateTime? lastConsumedDate=null, Rating? rating=null);

        Task<bool> Update(Guid id, string title, Guid ganreId, DateTime publishedDate, Guid authorId, string? description = null, DateTime? lastConsumedDate = null, Rating? rating = null);

        Task<bool> Remove(Guid id);

        Task<IEnumerable<Media>> GetAll();

        Task<IEnumerable<Media>> GetByAuthorId(Guid authorId);

        Task<IEnumerable<Media>> GetByGanreId(Guid ganreId);

        Task<IEnumerable<Media>> GetByMediaTypeId(Guid mediaTypeId);

        Task<Media?> GetById(Guid id);

        Task<bool> Rate(Guid id, Rating rating);

        Task<bool> SetConsumed(Guid id);


    }
}
