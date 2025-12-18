using AutoMapper;
using MediaControlApp.Application.Services.Interfaces;
using MediaControlApp.Domain.Models.Media;
using Microsoft.EntityFrameworkCore;


namespace MediaControlApp.Infrastructure.DataAccess.MediaStore.Repositories
{
    public class GanreRepo : IGanreRepo
    {
        private readonly IMapper _mapper;
        private readonly MediaDbContext _context;

        public GanreRepo(IMapper mapper, MediaDbContext dbContext)
        {
            _mapper = mapper;
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
            var authorList = await _context.Ganres.ToListAsync();

            return _mapper.Map<IEnumerable<Ganre>, IEnumerable<Ganre>>(authorList);
        }

        public async Task<Ganre?> GetById(Guid id)
        {
            var ganre = await _context.Ganres.SingleOrDefaultAsync(a => a.Id == id);
            if (ganre == null) { return null; }
            return _mapper.Map<Ganre, Ganre>(ganre);
        }

        public async Task<IEnumerable<Ganre>> GetByMediaTypeId(Guid mediaTypeId)
        {
            var authorList = await _context.Ganres.Where(g=>g.MediaTypeId==mediaTypeId).ToListAsync();

            return _mapper.Map<IEnumerable<Ganre>, IEnumerable<Ganre>>(authorList);
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
