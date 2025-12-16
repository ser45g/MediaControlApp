using MediaControlApp.Domain.Models.Media;
using System;
using System.Collections.Generic;
using System.Text;

namespace MediaControlApp.Application.Services.Interfaces
{
    public interface IMediaTypeRepo
    {
        Task<bool> Add(string name);

        Task<bool> Update(Guid id, string name);

        Task<bool> Remove(Guid id);

        Task<IEnumerable<MediaType>> GetAll();
        Task<MediaType?> GetById(Guid id);
    }
}
