using MediaControlApp.Application.Services.Interfaces;
using MediaControlApp.Domain.Models.Media;
using System;
using System.Collections.Generic;
using System.Text;

namespace MediaControlApp.Application.Services
{
    public class AuthorService
    {
        private readonly IAuthorRepo _authorRepo;

        public AuthorService(IAuthorRepo authorRepo)
        {
            _authorRepo = authorRepo;
        }

        public async Task<Author> Add(string firstName, string lastName, string? companyName = null, string? email = null)
        {
            return await _authorRepo.Add(firstName:firstName, lastName:lastName, companyName:companyName, email:email);
        }
        public async Task<Author> Update(Guid id, string firstName, string lastName, string? companyName = null, string? email = null)
        {
            return await _authorRepo.Update(id:id,firstName:firstName,lastName:lastName,companyName:companyName,email:email);
        }
        public async Task<bool> Remove(Guid id)
        {
            return await _authorRepo.Remove(id);
        }
        public async Task<Author> GetById(Guid id)
        {
            return await _authorRepo.GetById(id);
        }
        public async Task<IEnumerable<Author>> GetAll()
        {
            return await _authorRepo.GetAll();
        }
    }
}
