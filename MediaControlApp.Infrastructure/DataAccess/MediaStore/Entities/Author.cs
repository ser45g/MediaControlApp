namespace MediaControlApp.Infrastructure.DataAccess.MediaStore.Entities
{
    public class Author
    {
      
        public Guid Id { get;  set; }
        public string FirstName { get;  set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        public string? CompanyName {  get; set; } = string.Empty;
        public string? Email { get; set; }
    }
}
