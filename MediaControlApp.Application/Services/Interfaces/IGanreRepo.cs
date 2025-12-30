using MediaControlApp.Domain.Models.Media;


namespace MediaControlApp.Application.Services.Interfaces
{
    public interface IGanreRepo
    {
        Task<bool> Add(string name, Guid mediaTypeId, string? description = null, CancellationToken cancellationToken = default);
        Task<bool> Update(Guid id, string name, Guid mediaTypeId, string? description = null, CancellationToken cancellationToken = default);

        Task<bool> Remove(Guid id, CancellationToken cancellationToken = default);

        Task<IEnumerable<Ganre>> GetAll(CancellationToken cancellationToken = default);

        Task<Ganre?> GetById(Guid id, CancellationToken cancellationToken = default);

        Task<Ganre?> GetByName(string name, CancellationToken cancellationToken = default);

        Task<IEnumerable<Ganre>> GetByMediaTypeId(Guid mediaTypeId, CancellationToken cancellationToken = default);

    }
}
