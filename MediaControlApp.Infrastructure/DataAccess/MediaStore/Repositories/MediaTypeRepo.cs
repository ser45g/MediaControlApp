using AutoMapper;
using MediaControlApp.Application.Services.Interfaces;
using MediaControlApp.Domain.Models.Media;

using MediaControlApp.Infrastructure.DataAccess.MediaStore;
using Microsoft.EntityFrameworkCore;


namespace MediaControlApp.Infrastructure.DataAccess.MediaStore.Repositories
{
    public class MediaTypeRepo : IMediaTypeRepo
    {
        private readonly MediaDbContext _context;
        private readonly IMapper _mapper;

        public MediaTypeRepo(MediaDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> Add(string name)
        {
            var mediaType = new Entities.MediaType() { Name=name};
            _context.MediaTypes.Add(mediaType);
            return await _context.SaveChangesAsync() == 1;
        }

        public async Task<IEnumerable<MediaType>> GetAll()
        {
            var mediaTypeList = await _context.MediaTypes.ToListAsync();

            return _mapper.Map<IEnumerable<Entities.MediaType>, IEnumerable<MediaType>>(mediaTypeList);
        }

        public async Task<MediaType?> GetById(Guid id)
        {
            var mediaType = await _context.MediaTypes.SingleOrDefaultAsync(a => a.Id == id);
            if (mediaType == null) { return null; }
            return _mapper.Map<Entities.MediaType, MediaType>(mediaType);
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
