namespace MediaControlApp.Domain.Models.Media
{
    public class Author
    {
        public Author(Guid id, string firstName, string lastName, string? companyName, string? email)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            CompanyName = companyName;
        }

        public Guid Id { get; private set; }
        public string FirstName { get; private set; } = string.Empty;
        public string LastName { get; private set; } = string.Empty;

        public string? CompanyName {  get; private set; } = string.Empty;
    }
}
