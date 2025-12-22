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

        public async Task<Author?> GetByName(string name) { 
            return await _authorRepo.GetByName(name);

        }

        public async Task<Author?> GetByCompanyName(string companyName)
        {
            return await _authorRepo.GetByCompanyName(companyName);
        }

        public async Task<Author?> GetByEmail(string email)
        {
            return await _authorRepo.GetByEmail(email);
        }


        public async Task<IEnumerable<Author>> GetAll()
        {
            return await _authorRepo.GetAll();
        }
    }
}
