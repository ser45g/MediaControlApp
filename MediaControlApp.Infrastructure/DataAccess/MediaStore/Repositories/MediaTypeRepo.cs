using Dapper;
using Dommel;
using MediaControlApp.Application.Services.Interfaces;
using MediaControlApp.Domain.Models.Media;
using Microsoft.EntityFrameworkCore;


namespace MediaControlApp.Infrastructure.DataAccess.MediaStore.Repositories
{
    public class MediaTypeRepo : IMediaTypeRepo
    {
        private readonly MediaDbContextDapper _context;


        public MediaTypeRepo(MediaDbContextDapper context)
        {
            _context = context;
        }

        public async Task<bool> Add(string name, CancellationToken cancellationToken = default)
        {
            var mediaType = new MediaType() { Name=name};
            using var connection = _context.CreateConnection();

            connection.Open();

            //var res = await connection.InsertAsync<MediaType>(mediaType, cancellationToken: cancellationToken);

            var sql = "insert into mediaTypes (Id, Name) values (@Id, @Name )";

            var res = await connection.ExecuteAsync(sql, new {Id=Guid.NewGuid().ToString(),Name=name});
          
            return res>0;
        }

        public async Task<IEnumerable<MediaType>> GetAll(CancellationToken cancellationToken = default)
        {
            using var connection = _context.CreateConnection();

            connection.Open();

            var res = await connection.GetAllAsync<MediaType>(cancellationToken: cancellationToken);
            
            return res;
        }

        public async Task<MediaType?> GetById(Guid id, CancellationToken cancellationToken = default)
        {
            using var connection = _context.CreateConnection();

            connection.Open();

            var res = await connection.GetAsync<MediaType>(id, cancellationToken: cancellationToken);

            return res;
        }

        public async Task<MediaType?> GetByName(string name, CancellationToken cancellationToken = default)
        {
            using var connection = _context.CreateConnection();

            connection.Open();

            var res = await connection.FirstOrDefaultAsync<MediaType>(mt=>mt.Name==name, cancellationToken: cancellationToken);

            return res;
        }

        public async Task<bool> Remove(Guid id, CancellationToken cancellationToken = default)
        {
            using var connection = _context.CreateConnection();

            connection.Open();

            var res = await connection.ExecuteAsync("delete * from mediaTypes where Id=@id", id);

            return res == 1;
        }

        public async Task<bool> Update(Guid id, string name, CancellationToken cancellationToken = default)
        {
            var mediaType = new MediaType() { Id = id, Name = name };

            using var connection = _context.CreateConnection();

            connection.Open();

            return await connection.UpdateAsync<MediaType>(mediaType, cancellationToken: cancellationToken);

        }
    }
}
