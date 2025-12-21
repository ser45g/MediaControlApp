using MediaControlApp.Domain.Models.Media;
using System;
using System.Collections.Generic;
using System.Text;

namespace MediaControlApp.Application.Services.Interfaces
{
    public interface IAuthorRepo
    {
        Task<bool> Add(string name, string? companyName = null, string? email = null);
        Task<bool> Update(Guid id, string name, string? companyName = null, string? email = null);
        Task<bool> Remove(Guid id);
        Task<Author?> GetById(Guid id);

        Task<Author?> GetByName(string name);

        Task<Author?> GetByCompanyName(string companyName);

        Task<Author?> GetByEmail(string email);

        Task<IEnumerable<Author>> GetAll(); 

    }
}
