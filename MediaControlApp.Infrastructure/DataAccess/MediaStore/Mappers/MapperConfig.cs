using AutoMapper;
using MediaControlApp.Domain.Models.Media;
namespace MediaControlApp.Infrastructure.DataAccess.MediaStore.Mappers
{
    public class MapperConfig:Profile
    {
        public MapperConfig() {
            CreateMap<Ganre, Entities.Ganre>();

            CreateMap<Author, Entities.Author>();

            CreateMap<MediaType,Entities.MediaType>();

            CreateMap<Media, Entities.Media>().ForMember(dest=>dest.Rating, opt=>opt.MapFrom(src=>src.Rating.Value));
            
        }
    }
}
