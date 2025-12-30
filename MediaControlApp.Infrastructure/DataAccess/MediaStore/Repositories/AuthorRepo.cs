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

        public async Task<bool> Add(string name, string? companyName = null, string? email = null, CancellationToken cancellationToken = default)
        {
            var author = new Author() { Name=name,CompanyName=companyName, Email = email };
            _context.Authors.Add(author);
            return await _context.SaveChangesAsync(cancellationToken)==1;
        }

        public async Task<IEnumerable<Author>> GetAll(CancellationToken cancellationToken = default)
        {
            return await _context.Authors.ToListAsync(cancellationToken);
        }

        public async Task<Author?> GetByCompanyName(string companyName, CancellationToken cancellationToken = default)
        {
            return await _context.Authors.SingleOrDefaultAsync(a => a.CompanyName == companyName, cancellationToken);
        }

        public async Task<Author?> GetByEmail(string email, CancellationToken cancellationToken = default)
        {
            return await _context.Authors.SingleOrDefaultAsync(a => a.Email == email, cancellationToken);
        }

        public async Task<Author?> GetById(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Authors.SingleOrDefaultAsync(a => a.Id == id, cancellationToken);
           
        }

        public async Task<Author?> GetByName(string name, CancellationToken cancellationToken = default)
        {
            return await _context.Authors.SingleOrDefaultAsync(a => a.Name == name, cancellationToken);
        }

        public async Task<bool> Remove(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Authors.Where(m => m.Id == id).ExecuteDeleteAsync(cancellationToken) == 1;
        }

        public async Task<bool> Update(Guid id, string name, string? companyName = null, string? email = null,  CancellationToken cancellationToken = default)
        {
            return await _context.Authors.Where(m => m.Id == id).ExecuteUpdateAsync((m) => m.SetProperty(m => m.Name, name).SetProperty(m => m.CompanyName, companyName).SetProperty(m => m.Email, email),cancellationToken) == 1;
        }
    }
}
