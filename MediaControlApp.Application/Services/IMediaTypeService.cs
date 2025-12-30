using MediaControlApp.Domain.Models.Media;

namespace MediaControlApp.Application.Services
{
    public interface IMediaTypeService
    {
        Task<bool> Add(string name, CancellationToken cancellationToken = default);
        Task<IEnumerable<MediaType>> GetAll(CancellationToken cancellationToken = default);
        Task<MediaType?> GetById(Guid id, CancellationToken cancellationToken = default);
        Task<MediaType?> GetByName(string name, CancellationToken cancellationToken = default);
        Task<bool> Remove(Guid id, CancellationToken cancellationToken = default);
        Task<bool> Update(Guid id, string name, CancellationToken cancellationToken = default);
    }
}