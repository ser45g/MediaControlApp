using Dapper;
using Dommel;
using MediaControlApp.Domain.Models.Media;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MediaControlApp.Infrastructure.DataAccess.MediaStore.Utility
{
    public class UtilityDataRepo : IUtilityDataRepo
    {
        private readonly MediaDbContextDapper _context;

        public UtilityDataRepo(MediaDbContextDapper context)
        {
            _context = context;
        }

        public async Task<AmountsOfElements> GetAmountsOfElements()
        {
            using var connection = _context.CreateConnection();
            
            connection.Open();


            var multiResult = await connection.QueryMultipleAsync("""
                select count(*) from mediaTypes;
                select count(*) from medias;
                select count(*) from authors;
                select count(*) from ganres
                """);

            int mediaTypesAmount = multiResult.ReadSingle<int>();
            int authorAmount = multiResult.ReadSingle<int>();
            int mediaAmount = multiResult.ReadSingle<int>();
            int ganreAmount = multiResult.ReadSingle<int>();

            return new AmountsOfElements(mediaTypesAmount, mediaAmount, ganreAmount, authorAmount);
        }

    }
}
