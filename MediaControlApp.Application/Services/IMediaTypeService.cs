using MediaControlApp.Domain.Models.Media;

namespace MediaControlApp.Application.Services
{
    public interface IMediaTypeService
    {
        Task<bool> Add(string name);
        Task<IEnumerable<MediaType>> GetAll();
        Task<MediaType?> GetById(Guid id);
        Task<MediaType?> GetByName(string name);
        Task<bool> Remove(Guid id);
        Task<bool> Update(Guid id, string name);
    }
}