using MediaControlApp.Domain.Models.Media.ValueObjects;


namespace MediaControlApp.Domain.Models.Media
{
    public class Media
    {
        public Media(Guid id, string title, string? description, Ganre ganre, DateTime publisedDate, DateTime? lastConsumedDate, Author author, Action<DateTime>? onConsumed, Rating? rating)
        {
            Id = id;
            Title = title;
            Description = description;
            Ganre = ganre;
            PublisedDateUtc = publisedDate;
            LastConsumedDateUtc = lastConsumedDate;
            Author = author;
            OnConsumed = onConsumed;
            Rating = rating;
        }

        public Guid Id { get; private set; }

        public string Title { get; private set; } = string.Empty;

        public string? Description { get; private set; } = string.Empty;
        
        public Guid GanreId { get; private set; }
        public Ganre Ganre { get; private set; }

        public DateTime PublisedDateUtc { get; private set; }

        public DateTime? LastConsumedDateUtc { get; private set; }

        public Guid authorId { get; private set; }
        public Author? Author { get; private set; }

        public Action<DateTime>? OnConsumed { get; set; }

        public Rating? Rating { get; private set; }

        

        public void SetConsumed()
        {
            var dateTime = DateTime.UtcNow;
            LastConsumedDateUtc = dateTime;
            OnConsumed?.Invoke(dateTime);
        }

        public bool Rate(int score)
        {
            try
            {
                Rating = new Rating(score);
                return true;
            }
            catch (Exception ex) { 
                return false;
            }
             
        }
    }
}
