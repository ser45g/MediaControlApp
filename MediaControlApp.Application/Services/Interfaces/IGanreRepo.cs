using MediaControlApp.Domain.Models.Media;


namespace MediaControlApp.Application.Services.Interfaces
{
    public interface IGanreRepo
    {
        Task<bool> Add(string name, Guid mediaTypeId, string? description = null);
        Task<bool> Update(Guid id, string name, Guid mediaTypeId, string? description = null);

        Task<bool> Remove(Guid id);

        Task<IEnumerable<Ganre>> GetAll();

        Task<Ganre?> GetById(Guid id);

        Task<Ganre?> GetByName(string name);

        Task<IEnumerable<Ganre>> GetByMediaTypeId(Guid mediaTypeId);

    }
}
