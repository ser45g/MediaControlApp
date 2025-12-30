using MediaControlApp.Domain.Models.Media;

namespace MediaControlApp.Application.Services
{
    public interface IAuthorService
    {
        Task<bool> Add(string name, string? companyName = null, string? email = null, CancellationToken cancellationToken = default);
        Task<IEnumerable<Author>> GetAll(CancellationToken cancellationToken = default);
        Task<Author?> GetByCompanyName(string companyName, CancellationToken cancellationToken = default);
        Task<Author?> GetByEmail(string email, CancellationToken cancellationToken = default);
        Task<Author?> GetById(Guid id, CancellationToken cancellationToken = default);
        Task<Author?> GetByName(string name, CancellationToken cancellationToken = default);
        Task<bool> Remove(Guid id, CancellationToken cancellationToken = default);
        Task<bool> Update(Guid id, string name, string? companyName = null, string? email = null, CancellationToken cancellationToken = default);
    }
}