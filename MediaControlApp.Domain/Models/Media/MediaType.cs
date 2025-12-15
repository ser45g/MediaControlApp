namespace MediaControlApp.Domain.Models.Media
{
    public class MediaType
    {
        public MediaType(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public Guid Id { get;private set; }
        public string Name { get;private set; } = string.Empty;
  
    }
}
