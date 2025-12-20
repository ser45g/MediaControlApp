namespace MediaControlApp.Domain.Models.Media
{
    public class MediaType
    {
       

        public Guid Id { get;set; }
        public string Name { get;set; } = string.Empty;

        public IEnumerable<Ganre> Ganres { get; set; } = [];
  
    }
}
