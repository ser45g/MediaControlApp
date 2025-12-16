
namespace MediaControlApp.Infrastructure.DataAccess.MediaStore.Entities
{
    public class Ganre
    {
       

        public Guid Id { get; set; }
        public string Name { get; set; }=string.Empty;
        public string? Description { get; set; }= string.Empty;

        public Guid MediaTypeId { get; set; }
        public MediaType? MediaType { get;  set; }
    }
}
