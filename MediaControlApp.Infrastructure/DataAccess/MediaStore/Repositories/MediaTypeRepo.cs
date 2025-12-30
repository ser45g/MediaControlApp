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

        public async Task<bool> Add(string name, CancellationToken cancellationToken = default)
        {
            var mediaType = new MediaType() { Name=name};
            _context.MediaTypes.Add(mediaType);
            return await _context.SaveChangesAsync(cancellationToken) == 1;
        }

        public async Task<IEnumerable<MediaType>> GetAll(CancellationToken cancellationToken = default)
        {
            return await _context.MediaTypes.ToListAsync(cancellationToken);
        }

        public async Task<MediaType?> GetById(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.MediaTypes.SingleOrDefaultAsync(a => a.Id == id, cancellationToken);

        }

        public async Task<MediaType?> GetByName(string name, CancellationToken cancellationToken = default)
        {
            return await _context.MediaTypes.SingleOrDefaultAsync(a => a.Name==name, cancellationToken);
        }

        public async Task<bool> Remove(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.MediaTypes.Where(m => m.Id == id).ExecuteDeleteAsync(cancellationToken) == 1;
        }

        public async Task<bool> Update(Guid id, string name, CancellationToken cancellationToken = default)
        {
            return await _context.MediaTypes.Where(m => m.Id == id).ExecuteUpdateAsync((m) => m.SetProperty(m => m.Name, name), cancellationToken) == 1;
        }
    }
}
