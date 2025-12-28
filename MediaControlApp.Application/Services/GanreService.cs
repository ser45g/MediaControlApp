using MediaControlApp.Application.Services.Interfaces;
using MediaControlApp.Domain.Models.Media;


namespace MediaControlApp.Application.Services
{
    public class GanreService : IGanreService
    {
        public readonly IGanreRepo _ganreRepo;

        public GanreService(IGanreRepo genreRepo)
        {
            _ganreRepo = genreRepo;
        }

        public async Task<bool> Add(string name, Guid mediaTypeId, string? description = null)
        {
            return await _ganreRepo.Add(name: name, mediaTypeId: mediaTypeId, description: description);
        }
        public async Task<bool> Update(Guid id, string name, Guid mediaTypeId, string? description = null)
        {
            return await _ganreRepo.Update(id: id, name: name, mediaTypeId: mediaTypeId, description: description);
        }

        public async Task<bool> Remove(Guid id)
        {
            return await _ganreRepo.Remove(id);
        }

        public async Task<IEnumerable<Ganre>> GetAll()
        {
            return await _ganreRepo.GetAll();
        }

        public async Task<Ganre?> GetById(Guid id)
        {
            return await _ganreRepo.GetById(id);
        }
        public async Task<Ganre?> GetByName(string name)
        {
            return await _ganreRepo.GetByName(name);
        }

        public async Task<IEnumerable<Ganre>> GetByMediaTypeId(Guid mediaTypeId)
        {
            return await _ganreRepo.GetByMediaTypeId(mediaTypeId);
        }
    }
}
