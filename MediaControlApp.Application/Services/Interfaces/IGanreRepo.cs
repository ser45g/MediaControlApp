using MediaControlApp.Domain.Models.Media;
using System;
using System.Collections.Generic;
using System.Text;

namespace MediaControlApp.Application.Services.Interfaces
{
    public interface IGanreRepo
    {
        Task<Ganre> Add(string name, MediaType mediaType, string? description = null);
        Task<Ganre> Update(Guid id, string name, MediaType mediaType, string? description = null);

        Task<bool> Remove(Guid id);

        Task<IEnumerable<Ganre>> GetAll();

        Task<Ganre> GetById(Guid id);

        Task<IEnumerable<Ganre>> GetByMediaTypeId(Guid mediaTypeId);

    }
}
