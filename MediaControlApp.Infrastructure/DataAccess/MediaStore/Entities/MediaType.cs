namespace MediaControlApp.Infrastructure.DataAccess.MediaStore.Entities
{
    public class MediaType
    {
       
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;

        protected IEnumerable<Ganre> Ganres { get; set; } = [];
  
    }
}
