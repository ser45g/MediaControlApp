using MediaControlApp.Application.Services.Interfaces;
using MediaControlApp.Domain.Models.Media;
using System;
using System.Collections.Generic;
using System.Text;

namespace MediaControlApp.Application.Services
{
    public class GanreService
    {
        public readonly IGanreRepo _genreRepo;

        public GanreService(IGanreRepo genreRepo)
        {
            _genreRepo = genreRepo;
        }

        public async Task<Ganre> Add(string name, MediaType mediaType, string? description = null)
        {
            return await _genreRepo.Add(name:name,mediaType:mediaType,description:description);
        }
        public async Task<Ganre> Update(Guid id, string name, MediaType mediaType, string? description = null)
        {
            return await _genreRepo.Update(id: id, name: name, mediaType: mediaType, description: description);
        }

        public async Task<bool> Remove(Guid id)
        {
            return await _genreRepo.Remove(id);
        }

        public async Task<IEnumerable<Ganre>> GetAll()
        {
            return await _genreRepo.GetAll();
        }

        public async Task<Ganre> GetById(Guid id)
        {
            return await _genreRepo.GetById(id);
        }

        public async Task<IEnumerable<Ganre>> GetByMediaTypeId(Guid mediaTypeId)
        {
            return await _genreRepo.GetByMediaTypeId(mediaTypeId);
        }
    }
}
