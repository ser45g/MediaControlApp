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

        public async Task<bool> Add(string title, string? description, Guid ganreId, DateTime publishedDate, Guid authorId, Rating? rating)
        {
            return await _mediaRepo.Add(title: title, description: description, ganreId: ganreId, publishedDate: publishedDate, authorId: authorId, rating: rating);
        }

        public async Task<bool> Update(Guid id, string title, string? description, Guid ganreId, DateTime publishedDate, DateTime? lastConsumedDate, Guid authorId, Rating? rating)
        {
            return await _mediaRepo.Update(id: id, title: title, description: description, ganreId: ganreId, publishedDate: publishedDate, lastConsumedDate: lastConsumedDate, authorId: authorId, rating: rating);
        }

        public async Task<bool> Remove(Guid id)
        {
            return await _mediaRepo.Remove(id);
        }

        public async Task<IEnumerable<Media>> GetAll()
        {
            return await _mediaRepo.GetAll();
        }

        public async Task<Media?> GetByTitle(string title)
        {
            return await _mediaRepo.GetByTitle(title);
        }

        public async Task<IEnumerable<Media>> GetByAuthorId(Guid authorId)
        {
            return await _mediaRepo.GetByAuthorId(authorId);
        }

        public async Task<IEnumerable<Media>> GetByGanreId(Guid ganreId)
        {
            return await _mediaRepo.GetByGanreId(ganreId);
        }

        public async Task<IEnumerable<Media>> GetByMediaTypeId(Guid mediaTypeId)
        {
            return await _mediaRepo.GetByMediaTypeId(mediaTypeId);
        }

        public async Task<Media?> GetById(Guid id)
        {
            return await _mediaRepo.GetById(id);
        }

        public async Task<bool> SetConsumed(Guid id)
        {
            return await _mediaRepo.SetConsumed(id);

        }

        public async Task<bool> Rate(Guid id, Rating rating)
        {
            return await _mediaRepo.Rate(id, rating);
        }
    }
}
