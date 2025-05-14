using TravelApp.API.Models;

namespace TravelApp.Platform.Models
{
    public class AllUserInformation
    {
        public User User;
        public List<UserFeedback> Feedbacks;
        public List<FavoriteLocationItem> FavoriteLocations;
        public AllUserInformation(User user, List<UserFeedback> feedbacks, 
            List<FavoriteLocationItem> favoriteLocations)
        {
            User = user;
            Feedbacks = feedbacks;
            FavoriteLocations = favoriteLocations;
        }
    }
}
