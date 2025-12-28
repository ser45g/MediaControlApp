using MediaControlApp.Application.Services.Interfaces;
using MediaControlApp.Domain.Models.Media;
using Microsoft.EntityFrameworkCore;

namespace MediaControlApp.Infrastructure.DataAccess.MediaStore.Repositories
{
    public class AuthorRepo: IAuthorRepo
    {
        private readonly MediaDbContext _context;
       

        public AuthorRepo(MediaDbContext context)
        {
            _context = context;
           
        }

        public async Task<bool> Add(string name, string? companyName = null, string? email = null)
        {
            var author = new Author() { Name=name,CompanyName=companyName, Email = email };
            _context.Authors.Add(author);
            return await _context.SaveChangesAsync()==1;
        }

        public async Task<IEnumerable<Author>> GetAll()
        {
            return await _context.Authors.ToListAsync();
        }

        public async Task<Author?> GetByCompanyName(string companyName)
        {
            return await _context.Authors.SingleOrDefaultAsync(a => a.CompanyName == companyName);
        }

        public async Task<Author?> GetByEmail(string email)
        {
            return await _context.Authors.SingleOrDefaultAsync(a => a.Email == email);
        }

        public async Task<Author?> GetById(Guid id)
        {
            return await _context.Authors.SingleOrDefaultAsync(a => a.Id == id);
           
        }

        public async Task<Author?> GetByName(string name)
        {
            return await _context.Authors.SingleOrDefaultAsync(a => a.Name == name);
        }

        public async Task<bool> Remove(Guid id)
        {
            return await _context.Authors.Where(m => m.Id == id).ExecuteDeleteAsync() == 1;
        }

        public async Task<bool> Update(Guid id, string name, string? companyName = null, string? email = null)
        {
            return await _context.Authors.Where(m => m.Id == id).ExecuteUpdateAsync((m) => m.SetProperty(m => m.Name, name).SetProperty(m => m.CompanyName, companyName).SetProperty(m => m.Email, email)) == 1;
        }
    }
}
