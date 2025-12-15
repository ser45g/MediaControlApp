using MediaControlApp.Domain.Models.Media;
using System;
using System.Collections.Generic;
using System.Text;

namespace MediaControlApp.Application.Services.Interfaces
{
    public interface IAuthorRepo
    {
        Task<Author> Add(string firstName, string lastName, string? companyName = null, string? email = null);
        Task<Author> Update(Guid id, string firstName, string lastName, string? companyName = null, string? email = null);
        Task<bool> Remove(Guid id);
        Task<Author> GetById(Guid id);
        Task<IEnumerable<Author>> GetAll(); 

    }
}
