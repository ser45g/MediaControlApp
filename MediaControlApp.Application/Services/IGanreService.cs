using MediaControlApp.Domain.Models.Media;

namespace MediaControlApp.Application.Services
{
    public interface IGanreService
    {
        Task<bool> Add(string name, Guid mediaTypeId, string? description = null);
        Task<IEnumerable<Ganre>> GetAll();
        Task<Ganre?> GetById(Guid id);
        Task<IEnumerable<Ganre>> GetByMediaTypeId(Guid mediaTypeId);
        Task<Ganre?> GetByName(string name);
        Task<bool> Remove(Guid id);
        Task<bool> Update(Guid id, string name, Guid mediaTypeId, string? description = null);
    }
}