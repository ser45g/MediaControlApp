using MediaControlApp.Application.Services.Interfaces;
using MediaControlApp.Domain.Models.Media;


namespace MediaControlApp.Application.Services
{
    public class AuthorService
    {
        private readonly IAuthorRepo _authorRepo;

        public AuthorService(IAuthorRepo authorRepo)
        {
            _authorRepo = authorRepo;
        }

        public async Task<bool> Add(string name, string? companyName = null, string? email = null)
        {
            return await _authorRepo.Add(name:name, companyName:companyName, email:email);
        }
        public async Task<bool> Update(Guid id, string name, string? companyName = null, string? email = null)
        {
            return await _authorRepo.Update(id:id,name:name,companyName:companyName,email:email);
        }
        public async Task<bool> Remove(Guid id)
        {
            return await _authorRepo.Remove(id);
        }
        public async Task<Author?> GetById(Guid id)
        {
            return await _authorRepo.GetById(id);
        }
        public async Task<IEnumerable<Author>> GetAll()
        {
            return await _authorRepo.GetAll();
        }
    }
}
