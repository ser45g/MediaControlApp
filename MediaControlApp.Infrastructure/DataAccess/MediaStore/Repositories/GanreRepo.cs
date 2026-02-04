using Dapper;
using Dommel;
using MediaControlApp.Application.Services.Interfaces;
using MediaControlApp.Domain.Models.Media;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;


namespace MediaControlApp.Infrastructure.DataAccess.MediaStore.Repositories
{
    public class GanreRepo : IGanreRepo
    {
        private readonly MediaDbContextDapper _context;

        public GanreRepo(MediaDbContextDapper dbContext)
        {
            _context = dbContext;
        }

        public async Task<bool> Add(string name, Guid mediaTypeId, string? description = null, CancellationToken cancellationToken = default)
        {
            var ganre = new Ganre() { Name=name, Description=description, MediaTypeId=mediaTypeId};

            using var connection = _context.CreateConnection();

            connection.Open();

            var res = await connection.InsertAsync<Ganre>(ganre, cancellationToken: cancellationToken);

            return res != null;
        }

        public async Task<IEnumerable<Ganre>> GetAll(CancellationToken cancellationToken = default)
        {
            using var connection = _context.CreateConnection();

            connection.Open();

            var res = await connection.GetAllAsync<Ganre>(cancellationToken: cancellationToken);

            return res;
        }

        public async Task<Ganre?> GetById(Guid id, CancellationToken cancellationToken = default)
        {
            using var connection = _context.CreateConnection();

            connection.Open();

            var res = await connection.GetAsync<Ganre>(id, cancellationToken: cancellationToken);

            return res;
        }

        public async Task<IEnumerable<Ganre>> GetByMediaTypeId(Guid mediaTypeId, CancellationToken cancellationToken = default)
        {

            using var connection = _context.CreateConnection();

            connection.Open();

            var res = await connection.SelectAsync<Ganre>(g => g.MediaTypeId == mediaTypeId, cancellationToken: cancellationToken);

            return res;
        }

        public async Task<Ganre?> GetByName(string name, CancellationToken cancellationToken = default)
        {
            using var connection = _context.CreateConnection();

            connection.Open();

            var res = await connection.FirstOrDefaultAsync<Ganre>(mt => mt.Name == name, cancellationToken: cancellationToken);

            return res;
        }

        public async Task<bool> Remove(Guid id, CancellationToken cancellationToken = default)
        {
            using var connection = _context.CreateConnection();

            connection.Open();

            var res = await connection.ExecuteAsync("delete * from ganres where Id=@id", id);

            return res == 1;
        }

        public async Task<bool> Update(Guid id, string name, Guid mediaTypeId, string? description = null, CancellationToken cancellationToken = default)
        {
            var ganre = new Ganre() { Id = id, Name = name, MediaTypeId=mediaTypeId, Description= description};

            using var connection = _context.CreateConnection();

            connection.Open();

            return await connection.UpdateAsync<Ganre>(ganre, cancellationToken: cancellationToken);

        }
    }
}
