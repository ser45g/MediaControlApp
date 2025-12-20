namespace MediaControlApp.Domain.Models.Media
{
    public class Author
    {

        public Guid Id { get;  set; }
        public string FirstName { get;  set; } = string.Empty;
        public string LastName { get;  set; } = string.Empty;

        public string? Email { get;  set; } = string.Empty;

        public string? CompanyName {  get;  set; } = string.Empty;

        public IEnumerable<Media> Medias { get; set; } = Enumerable.Empty<Media>();
    }
}
