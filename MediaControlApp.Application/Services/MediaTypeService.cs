using MediaControlApp.Application.Services.Interfaces;
using MediaControlApp.Domain.Models.Media;
using System;
using System.Collections.Generic;
using System.Text;

namespace MediaControlApp.Application.Services
{
    public class MediaTypeService
    {
        public readonly IMediaTypeRepo _mediaTypeRepo;

        public MediaTypeService(IMediaTypeRepo mediaTypeRepo)
        {
            _mediaTypeRepo = mediaTypeRepo;
        }

        public async Task<MediaType> Add(string name)
        {
            return await _mediaTypeRepo.Add(name:name);
        }

        public async Task<MediaType> Update(Guid id, string name)
        {
            return await _mediaTypeRepo.Update(id:id, name:name);
        }

        public async Task<bool> Remove(Guid id)
        {
            return await _mediaTypeRepo.Remove(id);
        }

        public async Task<IEnumerable<MediaType>> GetAll()
        {
            return await _mediaTypeRepo.GetAll(); 
        }
        public async Task<MediaType> GetById(Guid id)
        {
            return await _mediaTypeRepo.GetById(id);
        }
    }
}
