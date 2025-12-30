using MediaControlApp.Application.Services.Interfaces;
using MediaControlApp.Domain.Models.Media;
using MediaControlApp.Domain.Models.Media.ValueObjects;

namespace MediaControlApp.Application.Services
{
    public class MediaService : IMediaService
    {
        private readonly IMediaRepo _mediaRepo;

        public MediaService(IMediaRepo mediaRepo)
        {
            _mediaRepo = mediaRepo;
        }

        public async Task<bool> Add(string title, string? description, Guid ganreId, DateTime publishedDate, Guid authorId, Rating? rating, CancellationToken cancellationToken = default)
        {
            return await _mediaRepo.Add(title: title, description: description, ganreId: ganreId, publishedDate: publishedDate, authorId: authorId, rating: rating, cancellationToken:cancellationToken);
        }

        public async Task<bool> Update(Guid id, string title, string? description, Guid ganreId, DateTime publishedDate, DateTime? lastConsumedDate, Guid authorId, Rating? rating, CancellationToken cancellationToken = default)
        {
            return await _mediaRepo.Update(id: id, title: title, description: description, ganreId: ganreId, publishedDate: publishedDate, lastConsumedDate: lastConsumedDate, authorId: authorId, rating: rating, cancellationToken:cancellationToken);
        }

        public async Task<bool> Remove(Guid id, CancellationToken cancellationToken = default)
        {
            return await _mediaRepo.Remove(id, cancellationToken);
        }

        public async Task<IEnumerable<Media>> GetAll(CancellationToken cancellationToken = default)
        {
            return await _mediaRepo.GetAll(cancellationToken);
        }

        public async Task<Media?> GetByTitle(string title, CancellationToken cancellationToken = default)
        {
            return await _mediaRepo.GetByTitle(title, cancellationToken);
        }

        public async Task<IEnumerable<Media>> GetByAuthorId(Guid authorId, CancellationToken cancellationToken = default)
        {
            return await _mediaRepo.GetByAuthorId(authorId, cancellationToken);
        }

        public async Task<IEnumerable<Media>> GetByGanreId(Guid ganreId, CancellationToken cancellationToken = default)
        {
            return await _mediaRepo.GetByGanreId(ganreId, cancellationToken);
        }

        public async Task<IEnumerable<Media>> GetByMediaTypeId(Guid mediaTypeId, CancellationToken cancellationToken = default)
        {
            return await _mediaRepo.GetByMediaTypeId(mediaTypeId, cancellationToken);
        }

        public async Task<Media?> GetById(Guid id, CancellationToken cancellationToken = default)
        {
            return await _mediaRepo.GetById(id, cancellationToken);
        }

        public async Task<bool> SetConsumed(Guid id, CancellationToken cancellationToken = default)
        {
            return await _mediaRepo.SetConsumed(id, cancellationToken);

        }

        public async Task<bool> Rate(Guid id, Rating rating, CancellationToken cancellationToken = default)
        {
            return await _mediaRepo.Rate(id, rating, cancellationToken);
        }
    }
}
