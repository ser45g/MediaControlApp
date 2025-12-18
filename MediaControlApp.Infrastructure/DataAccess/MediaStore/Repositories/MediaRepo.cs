using AutoMapper;
using MediaControlApp.Application.Services.Interfaces;
using MediaControlApp.Domain.Models.Media;
using MediaControlApp.Domain.Models.Media.ValueObjects;

using Microsoft.EntityFrameworkCore;

namespace MediaControlApp.Infrastructure.DataAccess.MediaStore.Repositories
{
    public class MediaRepo : IMediaRepo
    {
        private readonly MediaDbContext _context;
        private readonly IMapper _mapper;

        public MediaRepo(MediaDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> Add(string title, Guid ganreId, DateTime publisedDate, Guid authorId, string? description = null, DateTime? lastConsumedDate = null, Rating? rating = null)
        {
            var media = new Media() { Title = title, PublisedDateUtc = publisedDate, Description = description, LastConsumedDateUtc = lastConsumedDate, Rating = rating };

            _context.Medias.Add(media);

            return await _context.SaveChangesAsync()==1;
        }

        public async Task<IEnumerable<Media>> GetAll()
        {
            var mediaList = await _context.Medias.ToListAsync();

            return _mapper.Map<IEnumerable<Media>, IEnumerable<Media>>(mediaList);
        }

        public async Task<IEnumerable<Media>> GetByAuthorId(Guid authorId)
        {
            var mediaList = await _context.Medias.Where(media => media.AuthorId == authorId).ToListAsync();
            return _mapper.Map<IEnumerable<Media>, IEnumerable<Media>>(mediaList);
        }

        public async Task<IEnumerable<Media>> GetByGanreId(Guid ganreId)
        {
            var mediaList = await _context.Medias.Where(media => media.GanreId == ganreId).ToListAsync();
            return _mapper.Map<IEnumerable<Media>, IEnumerable<Media>>(mediaList);
        }

        public async Task<Media?> GetById(Guid id)
        {
            var media = await _context.Medias.Where(media => media.Id == id).FirstOrDefaultAsync();
            if (media == null)
                return null;
            return _mapper.Map<Media, Media>(media);
        }

        public async Task<IEnumerable<Media>> GetByMediaTypeId(Guid mediaTypeId)
        {
            var mediaList = await _context.Medias.Include(m=>m.Ganre).Where(m=>m.Ganre.MediaTypeId == mediaTypeId).ToListAsync();
            return _mapper.Map<IEnumerable<Media>, IEnumerable<Media>>(mediaList);
        }

        public async Task<bool> Rate(Guid id, Rating rating)
        {
            return await _context.Medias.Where(m => m.Id == id).ExecuteUpdateAsync((m) => m.SetProperty(m => m.Rating, rating)) == 1;
        }

        public async Task<bool> Remove(Guid id)
        {
            return await _context.Medias.Where(m => m.Id == id).ExecuteDeleteAsync()==1;
            
        }

        public async Task<bool> SetConsumed(Guid id)
        {
            return await _context.Medias.Where(m=>m.Id==id).ExecuteUpdateAsync((m)=>m.SetProperty(m=>m.LastConsumedDateUtc, DateTime.UtcNow))==1;
        }

        public async Task<bool> Update(Guid id, string title, Guid ganreId, DateTime publisedDate, Guid authorId, string? description = null, DateTime? lastConsumedDate = null, Rating? rating = null)
        {

            return await _context.Medias.Where(m=>m.Id == id).ExecuteUpdateAsync((m) => m.SetProperty(m => m.AuthorId, authorId).SetProperty(m => m.Title, title).SetProperty(m => m.Description, description).SetProperty(m => m.PublisedDateUtc, publisedDate).SetProperty(m => m.LastConsumedDateUtc, lastConsumedDate).SetProperty(m => m.GanreId, ganreId).SetProperty(m => m.Rating, rating)) == 1;


        }

      
    }
}
