using MediaControlApp.Domain.Models.Media;
using System;
using System.Collections.Generic;
using System.Text;

namespace MediaControlApp.Application.Services.Interfaces
{
    public interface IMediaTypeRepo
    {
        Task<bool> Add(string name, CancellationToken cancellationToken = default);

        Task<bool> Update(Guid id, string name, CancellationToken cancellationToken = default);

        Task<bool> Remove(Guid id, CancellationToken cancellationToken = default);

        Task<IEnumerable<MediaType>> GetAll(CancellationToken cancellationToken = default);

        Task<MediaType?> GetById(Guid id, CancellationToken cancellationToken = default);

        Task<MediaType?> GetByName(string name, CancellationToken cancellationToken = default);
    }
}
