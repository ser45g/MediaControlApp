using MediaControlApp.Application.Services.Interfaces;
using MediaControlApp.Domain.Models.Media;
using MediaControlApp.Domain.Models.Media.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace MediaControlApp.Application.Services
{
    public class MediaService
    {
        private readonly IMediaRepo _mediaRepo;

        public MediaService(IMediaRepo mediaRepo)
        {
            _mediaRepo = mediaRepo;
        }

        public async Task<Media> Add(string title, string? description, Ganre ganre, DateTime publishedDate, Author author, Rating? rating)
        {
            return await _mediaRepo.Add(title:title, description:description, ganre:ganre, publisedDate:publishedDate, author:author, rating:rating);
        }

        public async Task<Media> Update(Guid id, string title, string? description, Ganre ganre, DateTime publisedDate, DateTime? lastConsumedDate, Author author, Action<DateTime>? onConsumed, Rating? rating) {
            return await _mediaRepo.Update(id:id, title:title, description:description,ganre:ganre,publisedDate:publisedDate,lastConsumedDate:lastConsumedDate,author:author, rating:rating);
        }

        public async Task<bool> Remove(Guid id)
        {
            return await _mediaRepo.Remove(id);
        }

        public async Task<IEnumerable<Media>> GetAll()
        {
            return await _mediaRepo.GetAll();
        }

        public async Task<IEnumerable<Media>> GetByAuthorId(Guid authorId)
        {
            return await _mediaRepo.GetByAuthorId(authorId); 
        }

        public async Task<IEnumerable<Media>> GetByGanreId(Guid ganreId) {
            return await _mediaRepo.GetByGanreId(ganreId);
        }

        public async Task<IEnumerable<Media>> GetByMediaTypeId(Guid mediaTypeId)
        {
            return await _mediaRepo.GetByMediaTypeId(mediaTypeId);
        }

        public async Task<Media> GetById(Guid id)
        {
            return await _mediaRepo.GetById(id);
        }

        public async Task<Media> SetConsumed(Guid id)
        {
            var media = await _mediaRepo.GetById(id);
            media.SetConsumed();
            return await _mediaRepo.Update(id:media.Id, title:media.Title, ganre:media.Ganre, publisedDate:media.PublisedDateUtc, author:media.Author, description:media.Description,lastConsumedDate:media.LastConsumedDateUtc,rating:media.Rating);

        }
    }
}
