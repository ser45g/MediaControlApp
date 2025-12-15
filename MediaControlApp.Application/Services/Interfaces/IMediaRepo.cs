


using MediaControlApp.Domain.Models.Media;
using MediaControlApp.Domain.Models.Media.ValueObjects;

namespace MediaControlApp.Application.Services.Interfaces
{
    public interface IMediaRepo
    {
        Task<Media> Add(string title,  Ganre ganre, DateTime publisedDate,Author author,string? description=null, DateTime? lastConsumedDate=null, Rating? rating=null);

        Task<Media> Update(Guid id, string title, Ganre ganre, DateTime publisedDate, Author author, string? description = null, DateTime? lastConsumedDate = null, Rating? rating = null);

        Task<bool> Remove(Guid id);

        Task<IEnumerable<Media>> GetAll();

        Task<IEnumerable<Media>> GetByAuthorId(Guid authorId);

        Task<IEnumerable<Media>> GetByGanreId(Guid ganreId);

        Task<IEnumerable<Media>> GetByMediaTypeId(Guid mediaTypeId);

        Task<Media> GetById(Guid id);


    }
}
