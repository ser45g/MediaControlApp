using MediaControlApp.Application.Services.Interfaces;
using MediaControlApp.Domain.Models.Media;
using MediaControlApp.Domain.Models.Media.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace MediaControlApp.Infrastructure.DataAccess.MediaStore.Repositories
{
    public class MediaRepo : IMediaRepo
    {
        private readonly MediaDbContext _context;

        public MediaRepo(MediaDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Add(string title, Guid ganreId, DateTime publisedDate, Guid authorId, string? description = null, DateTime? lastConsumedDate = null, Rating? rating = null)
        {
            var media = new Media() { Title = title, PublishedDateUtc = publisedDate, Description = description, LastConsumedDateUtc = lastConsumedDate, Rating = rating };

            _context.Medias.Add(media);

            return await _context.SaveChangesAsync()==1;
        }

        public async Task<IEnumerable<Media>> GetAll()
        {
            return await _context.Medias.Include(m=>m.Ganre).Include(m=>m.Author).ToListAsync();
        }

        public async Task<IEnumerable<Media>> GetByAuthorId(Guid authorId)
        {
            return await _context.Medias.Include(m => m.Ganre).Include(m => m.Author).Where(media => media.AuthorId == authorId).ToListAsync();
        }

        public async Task<IEnumerable<Media>> GetByGanreId(Guid ganreId)
        {
            return await _context.Medias.Include(m => m.Ganre).Include(m => m.Author).Where(media => media.GanreId == ganreId).ToListAsync();
        }

        public async Task<Media?> GetById(Guid id)
        {
            return await _context.Medias.Include(m => m.Ganre).Include(m => m.Author).Where(media => media.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Media>> GetByMediaTypeId(Guid mediaTypeId)
        {
            return await _context.Medias.Include(m => m.Ganre).Include(m => m.Author).Where(m=> m.Ganre!=null && m.Ganre.MediaTypeId == mediaTypeId).ToListAsync();
        }

        public async Task<Media?> GetByTitle(string title)
        {

            return await _context.Medias.Include(m => m.Ganre).Include(m => m.Author).Where(media => media.Title == title).FirstOrDefaultAsync();
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

            return await _context.Medias.Where(m=>m.Id == id).ExecuteUpdateAsync((m) => m.SetProperty(m => m.AuthorId, authorId).SetProperty(m => m.Title, title).SetProperty(m => m.Description, description).SetProperty(m => m.PublishedDateUtc, publisedDate).SetProperty(m => m.LastConsumedDateUtc, lastConsumedDate).SetProperty(m => m.GanreId, ganreId).SetProperty(m => m.Rating, rating)) == 1;


        }

      
    }
}
