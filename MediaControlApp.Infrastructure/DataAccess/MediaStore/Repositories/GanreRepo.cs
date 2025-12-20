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

        public async Task<bool> Add(string name, Guid mediaTypeId, string? description = null)
        {
            var ganre = new Ganre() { Name=name, Description=description, MediaTypeId=mediaTypeId};
            _context.Ganres.Add(ganre);
            return await _context.SaveChangesAsync() == 1;
        }

        public async Task<IEnumerable<Ganre>> GetAll()
        {
            return await _context.Ganres.ToListAsync();
        }

        public async Task<Ganre?> GetById(Guid id)
        {
            return await _context.Ganres.SingleOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<Ganre>> GetByMediaTypeId(Guid mediaTypeId)
        {
            return await _context.Ganres.Where(g=>g.MediaTypeId==mediaTypeId).ToListAsync();
        }

        public async Task<bool> Remove(Guid id)
        {
            return await _context.Ganres.Where(m => m.Id == id).ExecuteDeleteAsync() == 1;
        }

        public async Task<bool> Update(Guid id, string name, Guid mediaTypeId, string? description = null)
        {
            return await _context.Ganres.Where(m => m.Id == id).ExecuteUpdateAsync((m) => m.SetProperty(m => m.Name, name).SetProperty(m => m.MediaTypeId, mediaTypeId).SetProperty(m => m.Description, description)) == 1;
        }
    }
}
