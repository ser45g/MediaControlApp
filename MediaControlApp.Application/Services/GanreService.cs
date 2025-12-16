using MediaControlApp.Application.Services.Interfaces;
using MediaControlApp.Domain.Models.Media;


namespace MediaControlApp.Application.Services
{
    public class GanreService
    {
        public readonly IGanreRepo _genreRepo;

        public GanreService(IGanreRepo genreRepo)
        {
            _genreRepo = genreRepo;
        }

        public async Task<bool> Add(string name, Guid mediaTypeId, string? description = null)
        {
            return await _genreRepo.Add(name:name,mediaTypeId:mediaTypeId,description:description);
        }
        public async Task<bool> Update(Guid id, string name, Guid mediaTypeId, string? description = null)
        {
            return await _genreRepo.Update(id: id, name: name, mediaTypeId: mediaTypeId, description: description);
        }

        public async Task<bool> Remove(Guid id)
        {
            return await _genreRepo.Remove(id);
        }

        public async Task<IEnumerable<Ganre>> GetAll()
        {
            return await _genreRepo.GetAll();
        }

        public async Task<Ganre?> GetById(Guid id)
        {
            return await _genreRepo.GetById(id);
        }

        public async Task<IEnumerable<Ganre>> GetByMediaTypeId(Guid mediaTypeId)
        {
            return await _genreRepo.GetByMediaTypeId(mediaTypeId);
        }
    }
}
