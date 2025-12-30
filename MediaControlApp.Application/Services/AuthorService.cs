using MediaControlApp.Application.Services.Interfaces;
using MediaControlApp.Domain.Models.Media;


namespace MediaControlApp.Application.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorRepo _authorRepo;

        public AuthorService(IAuthorRepo authorRepo)
        {
            _authorRepo = authorRepo;
        }

        public async Task<bool> Add(string name, string? companyName = null, string? email = null, CancellationToken cancellationToken = default)
        {
            return await _authorRepo.Add(name: name, companyName: companyName, email: email, cancellationToken: cancellationToken);
        }
        public async Task<bool> Update(Guid id, string name, string? companyName = null, string? email = null, CancellationToken cancellationToken = default)
        {
            return await _authorRepo.Update(id: id, name: name, companyName: companyName, email: email, cancellationToken: cancellationToken);
        }
        public async Task<bool> Remove(Guid id, CancellationToken cancellationToken = default)
        {
            return await _authorRepo.Remove(id, cancellationToken);
        }
        public async Task<Author?> GetById(Guid id, CancellationToken cancellationToken = default)
        {
            return await _authorRepo.GetById(id, cancellationToken);
        }

        public async Task<Author?> GetByName(string name, CancellationToken cancellationToken = default)
        {
            return await _authorRepo.GetByName(name, cancellationToken);
        }

        public async Task<Author?> GetByCompanyName(string companyName, CancellationToken cancellationToken = default)
        {
            return await _authorRepo.GetByCompanyName(companyName, cancellationToken);
        }

        public async Task<Author?> GetByEmail(string email, CancellationToken cancellationToken = default)
        {
            return await _authorRepo.GetByEmail(email, cancellationToken);
        }


        public async Task<IEnumerable<Author>> GetAll(CancellationToken cancellationToken = default)
        {
            return await _authorRepo.GetAll(cancellationToken);
        }
    }
}
