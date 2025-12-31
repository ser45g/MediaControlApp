using MediaControlApp.Domain.Models.Media;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MediaControlApp.Infrastructure.DataAccess.MediaStore.Utility
{
    public class UtilityDataRepo : IUtilityDataRepo
    {
        private readonly MediaDbContext _context;


        public UtilityDataRepo(MediaDbContext context)
        {
            _context = context;
        }

        public async Task<AmountsOfElements> GetAmountsOfElements()
        {
            int mediaTypesAmount = _context.MediaTypes.Count();
            int authorAmount = _context.Medias.Count();
            int mediaAmount = _context.Ganres.Count();
            int ganreAmount = _context.Authors.Count();

            return new AmountsOfElements(mediaTypesAmount, mediaAmount, ganreAmount, authorAmount);
        }

    }
}
