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

        public async Task<bool> Add(string name, Guid mediaTypeId, string? description = null, CancellationToken cancellationToken = default)
        {
            return await _ganreRepo.Add(name: name, mediaTypeId: mediaTypeId, description: description);
        }
        public async Task<bool> Update(Guid id, string name, Guid mediaTypeId, string? description = null, CancellationToken cancellationToken = default)
        {
            return await _ganreRepo.Update(id: id, name: name, mediaTypeId: mediaTypeId, description: description);
        }

        public async Task<bool> Remove(Guid id, CancellationToken cancellationToken = default)
        {
            return await _ganreRepo.Remove(id);
        }

        public async Task<IEnumerable<Ganre>> GetAll(CancellationToken cancellationToken = default)
        {
            return await _ganreRepo.GetAll(cancellationToken);
        }

        public async Task<Ganre?> GetById(Guid id, CancellationToken cancellationToken = default)
        {
            return await _ganreRepo.GetById(id, cancellationToken);
        }
        public async Task<Ganre?> GetByName(string name, CancellationToken cancellationToken = default)
        {
            return await _ganreRepo.GetByName(name, cancellationToken);
        }

        public async Task<IEnumerable<Ganre>> GetByMediaTypeId(Guid mediaTypeId, CancellationToken cancellationToken = default)
        {
            return await _ganreRepo.GetByMediaTypeId(mediaTypeId, cancellationToken);
        }
    }
}
