using MediaControlApp.Domain.Models.Media;

namespace MediaControlApp.Application.Services
{
    public interface IAuthorService
    {
        Task<bool> Add(string name, string? companyName = null, string? email = null);
        Task<IEnumerable<Author>> GetAll();
        Task<Author?> GetByCompanyName(string companyName);
        Task<Author?> GetByEmail(string email);
        Task<Author?> GetById(Guid id);
        Task<Author?> GetByName(string name);
        Task<bool> Remove(Guid id);
        Task<bool> Update(Guid id, string name, string? companyName = null, string? email = null);
    }
}