namespace MediaControlApp.Infrastructure.DataAccess.MediaStore.Entities
{
    public class Media
    {
        public Guid Id { get;  set; }

        public string Title { get;  set; } = string.Empty;

        public string? Description { get; set; } = string.Empty;
        
        public Guid GanreId { get; set; }
        public Ganre? Ganre { get; set; }

        public DateTime PublisedDateUtc { get; set; }

        public DateTime? LastConsumedDateUtc { get; set; }

        public Guid AuthorId { get; set; }
        public Author? Author { get; set; }

        public double Rating { get; set; }

    }
}
