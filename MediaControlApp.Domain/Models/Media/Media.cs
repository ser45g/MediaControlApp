using MediaControlApp.Domain.Models.Media.ValueObjects;


namespace MediaControlApp.Domain.Models.Media
{
    public class Media
    {
     
        public Guid Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; } = string.Empty;
        
        public Guid GanreId { get; set; }
        public Ganre? Ganre { get; set; }

        public DateTime PublisedDateUtc { get; set; }

        public DateTime? LastConsumedDateUtc { get; set; }

        public Guid AuthorId { get; set; }
        public Author? Author { get; set; }

 

        public Rating? Rating { get; set; }

        

        public void SetConsumed()
        {
            var dateTime = DateTime.UtcNow;
            LastConsumedDateUtc = dateTime;
            
        }

        public bool Rate(int score)
        {
            try
            {
                Rating = new Rating(score);
                return true;
            }
            catch (Exception ) { 
                return false;
            }  
        }
    }
}
