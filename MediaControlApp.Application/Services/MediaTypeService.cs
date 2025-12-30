using MediaControlApp.Application.Services.Interfaces;
using MediaControlApp.Domain.Models.Media;


namespace MediaControlApp.Application.Services
{
    public class MediaTypeService : IMediaTypeService
    {
        public readonly IMediaTypeRepo _mediaTypeRepo;

        public MediaTypeService(IMediaTypeRepo mediaTypeRepo)
        {
            _mediaTypeRepo = mediaTypeRepo;
        }

        public async Task<bool> Add(string name, CancellationToken cancellationToken = default)
        {
            return await _mediaTypeRepo.Add(name: name, cancellationToken);
        }

        public async Task<bool> Update(Guid id, string name, CancellationToken cancellationToken = default)
        {
            return await _mediaTypeRepo.Update(id: id, name: name, cancellationToken);
        }

        public async Task<bool> Remove(Guid id, CancellationToken cancellationToken = default)
        {
            return await _mediaTypeRepo.Remove(id, cancellationToken);
        }

        public async Task<IEnumerable<MediaType>> GetAll(CancellationToken cancellationToken = default)
        {
            return await _mediaTypeRepo.GetAll(cancellationToken);
        }
        public async Task<MediaType?> GetById(Guid id, CancellationToken cancellationToken = default)
        {
            return await _mediaTypeRepo.GetById(id, cancellationToken);
        }

        public async Task<MediaType?> GetByName(string name, CancellationToken cancellationToken = default)
        {
            return await _mediaTypeRepo.GetByName(name, cancellationToken);
        }
    }
}
