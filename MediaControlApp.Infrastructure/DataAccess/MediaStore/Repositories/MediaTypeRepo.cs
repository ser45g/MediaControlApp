using MediaControlApp.Application.Services.Interfaces;
using MediaControlApp.Domain.Models.Media;
using Microsoft.EntityFrameworkCore;


namespace MediaControlApp.Infrastructure.DataAccess.MediaStore.Repositories
{
    public class MediaTypeRepo : IMediaTypeRepo
    {
        private readonly MediaDbContext _context;
       

        public MediaTypeRepo(MediaDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Add(string name)
        {
            var mediaType = new MediaType() { Name=name};
            _context.MediaTypes.Add(mediaType);
            return await _context.SaveChangesAsync() == 1;
        }

        public async Task<IEnumerable<MediaType>> GetAll()
        {
            return await _context.MediaTypes.ToListAsync();
        }

        public async Task<MediaType?> GetById(Guid id)
        {
            return await _context.MediaTypes.SingleOrDefaultAsync(a => a.Id == id);

        }

        public async Task<MediaType?> GetByName(string name)
        {
            return await _context.MediaTypes.SingleOrDefaultAsync(a => a.Name==name);
        }

        public async Task<bool> Remove(Guid id)
        {
            return await _context.MediaTypes.Where(m => m.Id == id).ExecuteDeleteAsync() == 1;
        }

        public async Task<bool> Update(Guid id, string name)
        {
            return await _context.MediaTypes.Where(m => m.Id == id).ExecuteUpdateAsync((m) => m.SetProperty(m => m.Name, name)) == 1;
        }
    }
}
