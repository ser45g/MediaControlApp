using Dapper;
using Dommel;
using MediaControlApp.Application.Services.Interfaces;
using MediaControlApp.Domain.Models.Media;
using MediaControlApp.Domain.Models.Media.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace MediaControlApp.Infrastructure.DataAccess.MediaStore.Repositories
{
    public class MediaRepo : IMediaRepo
    {
        private readonly MediaDbContextDapper _context;

        public MediaRepo(MediaDbContextDapper context)
        {
            _context = context;
        }

        public async Task<bool> Add(string title, Guid ganreId, DateTime publisedDate, Guid authorId, string? description = null, DateTime? lastConsumedDate = null, Rating? rating = null, CancellationToken cancellationToken = default)
        {
            var media = new Media() { Title = title, PublishedDateUtc = publisedDate, Description = description, LastConsumedDateUtc = lastConsumedDate, Rating = rating };

            using var connection = _context.CreateConnection();

            connection.Open();

            var res = await connection.InsertAsync<Media>(media, cancellationToken: cancellationToken);

            return res != null;
        }

        public async Task<IEnumerable<Media>> GetAll(CancellationToken cancellationToken = default)
        {
            using var connection = _context.CreateConnection();

            connection.Open();

            var res = await connection.GetAllAsync<Media>(cancellationToken: cancellationToken);

            return res;
        }

        public async Task<IEnumerable<Media>> GetByAuthorId(Guid authorId, CancellationToken cancellationToken = default)
        {
            using var connection = _context.CreateConnection();

            connection.Open();

            var res = await connection.SelectAsync<Media>(g => g.AuthorId == authorId, cancellationToken: cancellationToken);

            return res;
        }

        public async Task<IEnumerable<Media>> GetByGanreId(Guid ganreId, CancellationToken cancellationToken = default)
        {
            using var connection = _context.CreateConnection();

            connection.Open();

            var res = await connection.SelectAsync<Media>(g => g.GanreId == ganreId, cancellationToken: cancellationToken);

            return res;
        }

        public async Task<Media?> GetById(Guid id, CancellationToken cancellationToken = default)
        {
            using var connection = _context.CreateConnection();

            connection.Open();

            var res = await connection.GetAsync<Media>(id, cancellationToken: cancellationToken);

            return res;
        }

        public async Task<IEnumerable<Media>> GetByMediaTypeId(Guid mediaTypeId, CancellationToken cancellationToken = default)
        {
            return new List<Media>();
        }

        public async Task<Media?> GetByTitle(string title, CancellationToken cancellationToken = default)
        {

            return null;
        }

        public async Task<bool> Rate(Guid id, Rating rating, CancellationToken cancellationToken = default)
        {
            using var connection = _context.CreateConnection();

            connection.Open();

            using var transaction = connection.BeginTransaction();

            var media = await connection.GetAsync<Media>(id, transaction, cancellationToken: cancellationToken);
            if (media == null)
                return false;

            media.Rate(rating.Value);

            return await connection.UpdateAsync<Media>(media, transaction, cancellationToken: cancellationToken);
        }

        public async Task<bool> Remove(Guid id, CancellationToken cancellationToken = default)
        {
            using var connection = _context.CreateConnection();

            connection.Open();

            var res = await connection.ExecuteAsync("delete * from medias where Id=@id", id);

            return res == 1;
        }

        public async Task<bool> SetConsumed(Guid id, DateTime lastConsumed, CancellationToken cancellationToken = default)
        {
            using var connection = _context.CreateConnection();

            connection.Open();

            using var transaction = connection.BeginTransaction();

            var media = await connection.GetAsync<Media>(id, transaction, cancellationToken: cancellationToken);
            if (media == null)
                return false;

            media.SetConsumed();

            return await connection.UpdateAsync<Media>(media, transaction, cancellationToken: cancellationToken);
        }

        public async Task<bool> Update(Guid id, string title, Guid ganreId, DateTime publisedDate, Guid authorId, string? description = null, DateTime? lastConsumedDate = null, Rating? rating = null,  CancellationToken cancellationToken = default)
        {

            var media = new Media() { Id = id, Description = description, Title=title, GanreId = ganreId, AuthorId=authorId, LastConsumedDateUtc = lastConsumedDate, PublishedDateUtc= publisedDate, Rating=rating };

            using var connection = _context.CreateConnection();

            connection.Open();

            return await connection.UpdateAsync<Media>(media, cancellationToken: cancellationToken);
        }      
    }
}
