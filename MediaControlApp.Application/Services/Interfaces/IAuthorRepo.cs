using MediaControlApp.Domain.Models.Media;
using System;
using System.Collections.Generic;
using System.Text;

namespace MediaControlApp.Application.Services.Interfaces
{
    public interface IAuthorRepo
    {
        Task<bool> Add(string name, string? companyName = null, string? email = null, CancellationToken cancellationToken=default);
        Task<bool> Update(Guid id, string name, string? companyName = null, string? email = null, CancellationToken cancellationToken = default);
        Task<bool> Remove(Guid id, CancellationToken cancellationToken = default);
        Task<Author?> GetById(Guid id, CancellationToken cancellationToken = default);

        Task<Author?> GetByName(string name, CancellationToken cancellationToken = default);

        Task<Author?> GetByCompanyName(string companyName, CancellationToken cancellationToken = default);

        Task<Author?> GetByEmail(string email, CancellationToken cancellationToken = default);

        Task<IEnumerable<Author>> GetAll(CancellationToken cancellationToken = default); 

    }
}
