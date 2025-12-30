using MediaControlApp.Application.Services.Interfaces;
using MediaControlApp.Domain.Models.Media;
using Microsoft.EntityFrameworkCore;


namespace MediaControlApp.Infrastructure.DataAccess.MediaStore.Repositories
{
    public class GanreRepo : IGanreRepo
    {
        private readonly MediaDbContext _context;

        public GanreRepo(MediaDbContext dbContext)
        {
            _context = dbContext;
        }

        public async Task<bool> Add(string name, Guid mediaTypeId, string? description = null, CancellationToken cancellationToken = default)
        {
            var ganre = new Ganre() { Name=name, Description=description, MediaTypeId=mediaTypeId};
            _context.Ganres.Add(ganre);
            return await _context.SaveChangesAsync(cancellationToken) == 1;
        }

        public async Task<IEnumerable<Ganre>> GetAll(CancellationToken cancellationToken = default)
        {
            return await _context.Ganres.Include(g=>g.MediaType).ToListAsync(cancellationToken);
        }

        public async Task<Ganre?> GetById(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Ganres.Include(g => g.MediaType).SingleOrDefaultAsync(a => a.Id == id,cancellationToken);
        }

        public async Task<IEnumerable<Ganre>> GetByMediaTypeId(Guid mediaTypeId, CancellationToken cancellationToken = default)
        {

            return await _context.Ganres.Include(g => g.MediaType).Where(g=>g.MediaTypeId==mediaTypeId).ToListAsync(cancellationToken);
        }

        public async Task<Ganre?> GetByName(string name, CancellationToken cancellationToken = default)
        {
            return await _context.Ganres.Include(g => g.MediaType).FirstOrDefaultAsync(g => g.Name == name, cancellationToken);
        }

        public async Task<bool> Remove(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Ganres.Where(m => m.Id == id).ExecuteDeleteAsync(cancellationToken) == 1;
        }

        public async Task<bool> Update(Guid id, string name, Guid mediaTypeId, string? description = null, CancellationToken cancellationToken = default)
        {
            return await _context.Ganres.Where(m => m.Id == id).ExecuteUpdateAsync((m) => m.SetProperty(m => m.Name, name).SetProperty(m => m.MediaTypeId, mediaTypeId).SetProperty(m => m.Description, description), cancellationToken) == 1;
        }
    }
}
