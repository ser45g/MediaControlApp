namespace MediaControlApp.Domain.Models.Media
{
    public class Ganre
    {
        public Ganre(Guid id, string name,MediaType mediaType, string? description )
        {
            Id = id;
            Name = name;
            Description = description;
            MediaType = mediaType;
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; }=string.Empty;
        public string? Description { get; private set; }= string.Empty;

        public Guid MediaTypeId { get; private set; }
        public MediaType MediaType { get; private set; }
    }
}
