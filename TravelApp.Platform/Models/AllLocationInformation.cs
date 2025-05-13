using TravelApp.API.Models;

namespace TravelApp.Platform.Models
{
    public class AllLocationInformation
    {
        public LocationInHomePage Location { get; set; }
        public List<LocationGallery> Gallery { get; set; }
        public List<FeedbackView> Feedbacks { get; set; }
        public bool IsFavorites { get; set; }

        public AllLocationInformation(LocationInHomePage location, List<LocationGallery> gallery,
            List<FeedbackView> feedbacks, bool isFavorites)
        {
            Location = location;
            Gallery = gallery;
            Feedbacks = feedbacks;
            IsFavorites = isFavorites;
        }
    }
}
