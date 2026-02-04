using Dapper;
using Dommel;
using MediaControlApp.Application.Services.Interfaces;
using MediaControlApp.Domain.Models.Media;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace MediaControlApp.Infrastructure.DataAccess.MediaStore.Repositories
{
    public class AuthorRepo: IAuthorRepo
    {
        private readonly MediaDbContextDapper _context;
       

        public AuthorRepo(MediaDbContextDapper context)
        {
            _context = context;
           
        }

        public async Task<bool> Add(string name, string? companyName = null, string? email = null, CancellationToken cancellationToken = default)
        {
            var author = new Author() { Name=name,CompanyName=companyName, Email = email };

            using var connection = _context.CreateConnection();

            connection.Open();

            var res = await connection.InsertAsync<Author>(author, cancellationToken: cancellationToken);

            return res != null;
        }

        public async Task<IEnumerable<Author>> GetAll(CancellationToken cancellationToken = default)
        {
            using var connection = _context.CreateConnection();

            connection.Open();

            var res = await connection.GetAllAsync<Author>(cancellationToken: cancellationToken);

            return res;
        }

        public async Task<Author?> GetByCompanyName(string companyName, CancellationToken cancellationToken = default)
        {
            using var connection = _context.CreateConnection();

            connection.Open();

            var res = await connection.FirstOrDefaultAsync<Author>(mt => mt.CompanyName == companyName, cancellationToken: cancellationToken);

            return res;
        }

        public async Task<Author?> GetByEmail(string email, CancellationToken cancellationToken = default)
        {
            using var connection = _context.CreateConnection();

            connection.Open();

            var res = await connection.FirstOrDefaultAsync<Author>(mt => mt.Email == email, cancellationToken: cancellationToken);

            return res;
        }

        public async Task<Author?> GetById(Guid id, CancellationToken cancellationToken = default)
        {
            using var connection = _context.CreateConnection();

            connection.Open();

            var res = await connection.GetAsync<Author>(id, cancellationToken: cancellationToken);

            return res;
        }

        public async Task<Author?> GetByName(string name, CancellationToken cancellationToken = default)
        {
            using var connection = _context.CreateConnection();

            connection.Open();

            var res = await connection.FirstOrDefaultAsync<Author>(mt => mt.Name == name, cancellationToken: cancellationToken);

            return res;
        }

        public async Task<bool> Remove(Guid id, CancellationToken cancellationToken = default)
        {
            using var connection = _context.CreateConnection();

            connection.Open();

            var res = await connection.ExecuteAsync("delete * from authors where Id=@id", id);

            return res == 1;
        }

        public async Task<bool> Update(Guid id, string name, string? companyName = null, string? email = null,  CancellationToken cancellationToken = default)
        {
            var author = new Author() { Id = id, Name = name, CompanyName=companyName, Email=email };

            using var connection = _context.CreateConnection();

            connection.Open();

            return await connection.UpdateAsync<Author>(author, cancellationToken: cancellationToken);
        }
    }
}
